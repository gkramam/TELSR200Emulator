using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator
{
    public static class AppConfiguration
    {
        public static readonly int manipulatorPortNumber;
        public static readonly int preAlignerPortNumber;
        public static readonly int tcpWorkerLoopIdleTime;
        public static readonly int tcpBetweenCharacterTimeout;
        public static readonly bool useSequenceNumber;
        public static readonly bool checkSumCheck;
        public static readonly int endOfExecutionRetryTimeout;
        public static readonly bool useXmlFilesForReplies;
        public static readonly Configuration.Environment environment;
        public static Dictionary<string, Dictionary<string, string>> ManipulatorResponses;
        public static Dictionary<string, Dictionary<string, string>> PreAlignerResponses;
        
        public static Dictionary<string, Dictionary<string, string>> ManipulatorEoEs;
        public static Dictionary<string, Dictionary<string, string>> PreAlignerEoEs;
        static AppConfiguration() {

            manipulatorPortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit1Port"]);
            preAlignerPortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit2Port"]);
            tcpWorkerLoopIdleTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["tcpWorkerLoopIdleTime"]);
            tcpBetweenCharacterTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["tcpBetweenCharacterTimeout"]);
            useSequenceNumber = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["useSequenceNumber"]);
            checkSumCheck = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["checkSumCheck"]);
            endOfExecutionRetryTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["endOfExecutionRetryTimeout"]);
            useXmlFilesForReplies = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["useXmlFilesForReplies"]);

            XmlDocument doc = new XmlDocument();
            doc.Load("Environment.xml");
            environment = new Configuration.Environment(doc.SelectSingleNode("/Environment"));

            ManipulatorResponses = new Dictionary<string, Dictionary<string, string>>();
            ManipulatorEoEs = new Dictionary<string, Dictionary<string, string>>();
            PreAlignerResponses = new Dictionary<string, Dictionary<string, string>>();
            PreAlignerEoEs = new Dictionary<string, Dictionary<string, string>>();

            if (!useXmlFilesForReplies)
                return;

            DirectoryInfo di = new DirectoryInfo(@"Configuration");

            var files = di.GetFiles("*.xml", SearchOption.AllDirectories);

            foreach(var fi in files)
            {
                ProcessXMLFile(fi.FullName);
            }
            //if(di.Exists)
            //{
            //    var ManipulatorDir = di.GetDirectories("Manipulator").FirstOrDefault();
            //    if(ManipulatorDir!= null)
            //    {
            //        var manipResponseDir = ManipulatorDir.GetDirectories("Response").FirstOrDefault();
            //        if(manipResponseDir != null)
            //        {
            //            var manipulatorResponses = manipResponseDir.GetFiles("*.xml");
            //            foreach(var respFi in manipulatorResponses)
            //            {
            //                BaseResponse resp = null;
            //                switch(respFi.Name.Substring(0,12))
            //                {
            //                    case "ResponseMMAP":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMMAP), new object[] { null });
            //                        break;
            //                    case "ResponseCSRV":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseCSRV),new object[] { null});
            //                        break;
            //                    case "ResponseINIT":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseINIT),new object[] { null});
            //                        break;
            //                    case "ResponseMABS":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMABS),new object[] { null});
            //                        break;
            //                    case "ResponseMCTR":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMCTR),new object[] { null});
            //                        break;
            //                    case "ResponseMMCA":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMMCA),new object[] { null});
            //                        break;
            //                    case "ResponseMPNT":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMPNT),new object[] { null});
            //                        break;
            //                    case "ResponseMREL":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMREL),new object[] { null});
            //                        break;
            //                    case "ResponseMTCH":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMTCH),new object[] { null});
            //                        break;
            //                    case "ResponseMTRS":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMTRS),new object[] { null});
            //                        break;
            //                    case "ResponseRMAP":
            //                        resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRMAP), new object[] { null });
            //                        break;

            //                }
            //                if (resp != null)
            //                {
            //                    XmlDocument respDoc = new XmlDocument();
            //                    respDoc.Load(respFi.FullName);
            //                    var respDict = resp.ReadXML(respDoc);
            //                    ManipulatorResponses.Add(respFi.Name.Substring(8, 4), respDict);
            //                }
            //            }
            //        }
            //    }

            //    var manipulatorEoEDir = ManipulatorDir.GetDirectories("EndOfExecution").FirstOrDefault();
            //    if (manipulatorEoEDir != null)
            //    {
            //        var manipulatorEoEs = manipulatorEoEDir.GetFiles("*.xml");
            //        foreach (var eoeFi in manipulatorEoEs)
            //        {
            //            BaseResponse eoe = null;
            //            switch (eoeFi.Name.Substring(0, 13))
            //            {
            //                case "EndOfExecMMAP":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMMAP), new object[] { null });
            //                    break;
            //                case "EndOfExecCSRV":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecCSRV), new object[] { null });
            //                    break;
            //                case "EndOfExecINIT":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecINIT), new object[] { null });
            //                    break;
            //                case "EndOfExecMABS":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMABS), new object[] { null });
            //                    break;
            //                case "EndOfExecMCTR":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMCTR), new object[] { null });
            //                    break;
            //                case "EndOfExecMMCA":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMMCA), new object[] { null });
            //                    break;
            //                case "EndOfExecMPNT":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMPNT), new object[] { null });
            //                    break;
            //                case "EndOfExecMREL":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMREL), new object[] { null });
            //                    break;
            //                case "EndOfExecMTCH":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMTCH), new object[] { null });
            //                    break;
            //                case "EndOfExecMTRS":
            //                    eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMTRS), new object[] { null });
            //                    break;
            //            }
            //            if (eoe != null)
            //            {
            //                XmlDocument eoeDoc = new XmlDocument();
            //                eoeDoc.Load(eoeFi.FullName);
            //                var eoeDict = eoe.ReadXML(eoeDoc);
            //                ManipulatorEoEs.Add(eoeFi.Name.Substring(9, 4), eoeDict);
            //            }
            //        }
            //    }
            //}
        }

        static void ProcessXMLFile(string path)
        {
            FileInfo fi = new FileInfo(path);
            bool isManipulator = true;
            bool isResponse = false;
            bool isEoE = false;
            bool isEvent = false;
            if(fi.Exists)
            {
                var configPath = fi.FullName.Substring(Environment.CurrentDirectory.Length);
                var folders = configPath.Split('\\');
                
                if (folders[2].Equals("Manipulator"))
                    isManipulator = true;
                else if (folders[2].Equals("PreAligner"))
                    isManipulator = false;

                if (folders[3].Equals("Response"))
                    isResponse = true;
                else if (folders[3].Equals("EndOfExecution"))
                    isEoE = true;

                if(isResponse)
                {
                    BaseResponse resp = null;
                    if (isManipulator)
                    {
                        switch (fi.Name.Substring(0, 12))
                        {
                            case "ResponseMMAP":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMMAP), new object[] { null });
                                break;
                            case "ResponseCSRV":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseCSRV), new object[] { null });
                                break;
                            case "ResponseINIT":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseINIT), new object[] { null });
                                break;
                            case "ResponseMABS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMABS), new object[] { null });
                                break;
                            case "ResponseMCTR":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMCTR), new object[] { null });
                                break;
                            case "ResponseMMCA":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMMCA), new object[] { null });
                                break;
                            case "ResponseMPNT":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMPNT), new object[] { null });
                                break;
                            case "ResponseMREL":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMREL), new object[] { null });
                                break;
                            case "ResponseMTCH":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMTCH), new object[] { null });
                                break;
                            case "ResponseMTRS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseMTRS), new object[] { null });
                                break;
                            case "ResponseRMAP":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRMAP), new object[] { null });
                                break;

                        }
                    }
                    else
                    {
                        switch (fi.Name.Substring(0, 12))
                        {
                            case "ResponseINIT":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseINIT), new object[] { null });
                                break;
                            case "ResponseMABS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseMABS), new object[] { null });
                                break;
                            case "ResponseMACA":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseMACA), new object[] { null });
                                break;
                            case "ResponseMALN":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseMALN), new object[] { null });
                                break;
                            case "ResponseMREL":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseMREL), new object[] { null });
                                break;
                        }
                    }
                    if (resp != null)
                    {
                        XmlDocument respDoc = new XmlDocument();
                        respDoc.Load(fi.FullName);
                        var respDict = resp.ReadXML(respDoc);
                        if (isManipulator)
                        {
                            ManipulatorResponses.Remove(fi.Name.Substring(8, 4));
                            ManipulatorResponses.Add(fi.Name.Substring(8, 4), respDict);
                        }
                        else
                        {
                            PreAlignerResponses.Remove(fi.Name.Substring(8, 4));
                            PreAlignerResponses.Add(fi.Name.Substring(8, 4), respDict);
                        }
                    }
                }
                else
                {
                    BaseResponse eoe = null;
                    if (isManipulator)
                    {
                        switch (fi.Name.Substring(0, 13))
                        {
                            case "EndOfExecMMAP":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMMAP), new object[] { null });
                                break;
                            case "EndOfExecCSRV":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecCSRV), new object[] { null });
                                break;
                            case "EndOfExecINIT":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecINIT), new object[] { null });
                                break;
                            case "EndOfExecMABS":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMABS), new object[] { null });
                                break;
                            case "EndOfExecMCTR":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMCTR), new object[] { null });
                                break;
                            case "EndOfExecMMCA":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMMCA), new object[] { null });
                                break;
                            case "EndOfExecMPNT":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMPNT), new object[] { null });
                                break;
                            case "EndOfExecMREL":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMREL), new object[] { null });
                                break;
                            case "EndOfExecMTCH":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMTCH), new object[] { null });
                                break;
                            case "EndOfExecMTRS":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecMTRS), new object[] { null });
                                break;
                        }
                    }
                    else
                    {
                        switch (fi.Name.Substring(0, 13))
                        {
                            case "EndOfExecINIT":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecINIT), new object[] { null });
                                break;
                            case "EndOfExecMABS":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecMABS), new object[] { null });
                                break;
                            case "EndOfExecMACA":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecMACA), new object[] { null });
                                break;
                            case "EndOfExecMALN":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecMALN), new object[] { null });
                                break;
                            case "EndOfExecMREL":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecMREL), new object[] { null });
                                break;
                        }
                    }
                    if (eoe != null)
                    {
                        XmlDocument eoeDoc = new XmlDocument();
                        eoeDoc.Load(fi.FullName);
                        var eoeDict = eoe.ReadXML(eoeDoc);
                        if (isManipulator)
                        {
                            ManipulatorEoEs.Remove(fi.Name.Substring(9, 4));
                            ManipulatorEoEs.Add(fi.Name.Substring(9, 4), eoeDict);
                        }
                        else
                        {
                            PreAlignerEoEs.Remove(fi.Name.Substring(9, 4));
                            PreAlignerEoEs.Add(fi.Name.Substring(9, 4), eoeDict);
                        }
                    }
                }
            }
        }

    }
}

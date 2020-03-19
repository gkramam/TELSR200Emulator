using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TELSR200Emulator.Messages;
using TELSR200Emulator.Messages.PreAligner;

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
        static AppConfiguration()
        {

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

            foreach (var fi in files)
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
            if (fi.Exists)
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

                if (isResponse)
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
                                resp = (BaseResponse)Activator.CreateInstance(typeof(ResponseCSRV), new object[] { null });
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
                            case "ResponseCCLR":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseCCLR), new object[] { null });
                                break;
                            case "ResponseCRSM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseCRSM), new object[] { null });
                                break;
                            case "ResponseCSOL":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseCSOL), new object[] { null });
                                break;
                            case "ResponseCSTP":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseCSTP), new object[] { null });
                                break;
                            case "ResponseRAWC":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRAWC), new object[] { null });
                                break;
                            case "ResponseRERR":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRERR), new object[] { null });
                                break;
                            case "ResponseRLOG":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRLOG), new object[] { null });
                                break;
                            case "ResponseRMCA":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRMCA), new object[] { null });
                                break;
                            case "ResponseRMPD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRMPD), new object[] { null });
                                break;
                            case "ResponseRMSK":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRMSK), new object[] { null });
                                break;
                            case "ResponseRPOS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRPOS), new object[] { null });
                                break;
                            case "ResponseRPRM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRPRM), new object[] { null });
                                break;
                            case "ResponseRSLV":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRSLV), new object[] { null });
                                break;
                            case "ResponseRSPD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRSPD), new object[] { null });
                                break;
                            case "ResponseRSTP":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRSTP), new object[] { null });
                                break;
                            case "ResponseRSTR":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRSTR), new object[] { null });
                                break;
                            case "ResponseRSTS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRSTS), new object[] { null });
                                break;
                            case "ResponseRTRM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRTRM), new object[] { null });
                                break;
                            case "ResponseRVER":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseRVER), new object[] { null });
                                break;
                            case "ResponseSABS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSABS), new object[] { null });
                                break;
                            case "ResponseSAPS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSAPS), new object[] { null });
                                break;
                            case "ResponseSMSK":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSMSK), new object[] { null });
                                break;
                            case "ResponseSPDL":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSPDL), new object[] { null });
                                break;
                            case "ResponseSPLD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSPLD), new object[] { null });
                                break;
                            case "ResponseSPOS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSPOS), new object[] { null });
                                break;
                            case "ResponseSPRM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSPRM), new object[] { null });
                                break;
                            case "ResponseSPSV":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSPSV), new object[] { null });
                                break;
                            case "ResponseSSLV":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSSLV), new object[] { null });
                                break;
                            case "ResponseSSPD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSSPD), new object[] { null });
                                break;
                            case "ResponseSSTD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSSTD), new object[] { null });
                                break;
                            case "ResponseSSTR":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSSTR), new object[] { null });
                                break;
                            case "ResponseSTRM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.ResponseSTRM), new object[] { null });
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
                            case "ResponseCCLR":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseCCLR), new object[] { null });
                                break;
                            case "ResponseCRSM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseCRSM), new object[] { null });
                                break;
                            case "ResponseCSOL":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseCSOL), new object[] { null });
                                break;
                            case "ResponseCSRV":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseCSRV), new object[] { null });
                                break;
                            case "ResponseCSTP":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseCSTP), new object[] { null });
                                break;
                            case "ResponseRACA":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRACA), new object[] { null });
                                break;
                            case "ResponseRALN":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRALN), new object[] { null });
                                break;
                            case "ResponseRCCD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRCCD), new object[] { null });
                                break;
                            case "ResponseRERR":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRERR), new object[] { null });
                                break;
                            case "ResponseRLOG":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRLOG), new object[] { null });
                                break;
                            case "ResponseRMSK":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRMSK), new object[] { null });
                                break;
                            case "ResponseRPOS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRPOS), new object[] { null });
                                break;
                            case "ResponseRPRM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRPRM), new object[] { null });
                                break;
                            case "ResponseRSLV":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRSLV), new object[] { null });
                                break;
                            case "ResponseRSPD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRSPD), new object[] { null });
                                break;
                            case "ResponseRSTS":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRSTS), new object[] { null });
                                break;
                            case "ResponseRVER":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseRVER), new object[] { null });
                                break;
                            case "ResponseSMSK":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseSMSK), new object[] { null });
                                break;
                            case "ResponseSPRM":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseSPRM), new object[] { null });
                                break;
                            case "ResponseSSLV":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseSSLV), new object[] { null });
                                break;
                            case "ResponseSSPD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseSSPD), new object[] { null });
                                break;
                            case "ResponseSSTD":
                                resp = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.ResponseSSTD), new object[] { null });
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
                            case "EndOfExecCCLR":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecCCLR), new object[] { null });
                                break;
                            case "EndOfExecCRSM":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecCRSM), new object[] { null });
                                break;
                            case "EndOfExecCSOL":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecCSOL), new object[] { null });
                                break;
                            case "EndOfExecCSTP":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.Manipulator.EndOfExecCSTP), new object[] { null });
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
                            case "EndOfExecCCLR":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecCCLR), new object[] { null });
                                break;
                            case "EndOfExecCRSM":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecCRSM), new object[] { null });
                                break;
                            case "EndOfExecCSOL":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecCSOL), new object[] { null });
                                break;
                            case "EndOfExecCSRV":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecCSRV), new object[] { null });
                                break;
                            case "EndOfExecCSTP":
                                eoe = (BaseResponse)Activator.CreateInstance(typeof(Messages.PreAligner.EndOfExecCSTP), new object[] { null });
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

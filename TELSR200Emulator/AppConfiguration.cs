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

                BaseResponse reply = null;
                var reqType = BaseMessage.GetType(isManipulator ? 1 : 2, 
                                                    isResponse? fi.Name.Substring(8, 4): fi.Name.Substring(9, 4),
                                                    isResponse? CommandType.ReplyResponse: CommandType.ReplyEoE);

                reply = (BaseResponse)Activator.CreateInstance(reqType, new object[] { null });

                if (reply != null)
                {
                    XmlDocument replyDoc = new XmlDocument();
                    replyDoc.Load(fi.FullName);
                    var replyDict = reply.ReadXML(replyDoc);
                    if (isManipulator)
                    {
                        if (isResponse)
                        {
                            ManipulatorResponses.Remove(fi.Name.Substring(8, 4));
                            ManipulatorResponses.Add(fi.Name.Substring(8, 4), replyDict);
                        }
                        else
                        {
                            ManipulatorEoEs.Remove(fi.Name.Substring(9, 4));
                            ManipulatorEoEs.Add(fi.Name.Substring(9, 4), replyDict);
                        }
                    }
                    else
                    {
                        if (isResponse)
                        {
                            PreAlignerResponses.Remove(fi.Name.Substring(8, 4));
                            PreAlignerResponses.Add(fi.Name.Substring(8, 4), replyDict);
                        }
                        else
                        {
                            PreAlignerEoEs.Remove(fi.Name.Substring(9, 4));
                            PreAlignerEoEs.Add(fi.Name.Substring(9, 4), replyDict);
                        }
                    }
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        public static readonly Configuration.Environment environment;
        static AppConfiguration() {

            manipulatorPortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit1Port"]);
            preAlignerPortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit2Port"]);
            tcpWorkerLoopIdleTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["tcpWorkerLoopIdleTime"]);
            tcpBetweenCharacterTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["tcpBetweenCharacterTimeout"]);
            useSequenceNumber = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["useSequenceNumber"]);
            checkSumCheck = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["checkSumCheck"]);

            XmlDocument doc = new XmlDocument();
            doc.Load("Environment.xml");
            environment = new Configuration.Environment(doc.SelectSingleNode("/Environment"));
        }

    }
}

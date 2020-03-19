using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class EndOfExecGeneric : BaseEndOfExec
    {
        //TimeSpan _executionTime;

        public EndOfExecGeneric(BaseMessage request) : base(request)
        {
            //_executionTime = TimeSpan.FromMilliseconds(33);//33 mills
        }

        public override string Generate(Device device)
        {
            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                _responseBuilder.Append(xmlData["ExecutionTime"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["PositionData"]);
            }
            else
            {
                _responseBuilder.Append(_executionTime.ToString("ffffff"));
                _responseBuilder.Append(',');
                //_responseBuilder.Append("00000000");//pos1
                _responseBuilder.Append(((Devices.PreAligner)device).BuildEOEGeneric(_request));
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var executionTime = doc.GetElementsByTagName("ExecutionTime").Item(0).InnerText;
            var positionData = doc.GetElementsByTagName("PositionData").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("ExecutionTime", executionTime);
            ret.Add("PositionData", positionData);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

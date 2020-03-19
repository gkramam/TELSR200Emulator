using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecGeneric : BaseEndOfExec
    {

        public EndOfExecGeneric(BaseMessage request) : base(request)
        {
        }

        public override string Generate(Device device)
        {
            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                _responseBuilder.Append(xmlData["ExecutionTime"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["RotationAxis"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["ExtensionAxis"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["WristAxis1"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["WristAxis2"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["ElevationAxis"]);
            }
            else
            {
                var robot = (Devices.Manipulator)device;

                _responseBuilder.Append(_executionTime.ToString("ffffff"));
                _responseBuilder.Append(',');
                _responseBuilder.Append(robot.CurrentPositionPosture.ToString());
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var executionTime = doc.GetElementsByTagName("ExecutionTime").Item(0).InnerText;
            var positionData1 = doc.GetElementsByTagName("RotationAxis").Item(0).InnerText;
            var positionData2 = doc.GetElementsByTagName("ExtensionAxis").Item(0).InnerText;
            var positionData3 = doc.GetElementsByTagName("WristAxis1").Item(0).InnerText;
            var positionData4 = doc.GetElementsByTagName("WristAxis2").Item(0).InnerText;
            var positionData5 = doc.GetElementsByTagName("ElevationAxis").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("ExecutionTime", executionTime);
            ret.Add("RotationAxis", positionData1);
            ret.Add("ExtensionAxis", positionData2);
            ret.Add("WristAxis1", positionData3);
            ret.Add("WristAxis2", positionData4);
            ret.Add("ElevationAxis", positionData5);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

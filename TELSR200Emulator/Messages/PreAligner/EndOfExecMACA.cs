using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class EndOfExecMACA : BaseEndOfExec
    {
        public EndOfExecMACA(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                _responseBuilder.Append(xmlData["ExecutionTime"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["PositionData"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["CalibrationAngle"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["ManipulatorAdvanceAngle"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["DistanceBetweenCenters"]);
            }
            else
            {
                var preAligner = (Devices.PreAligner)device;

                _responseBuilder.Append(_executionTime.ToString("ffffff"));
                _responseBuilder.Append(',');
                _responseBuilder.Append(preAligner.BuildEOEMACA(_request));
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var executionTime = doc.GetElementsByTagName("ExecutionTime").Item(0).InnerText;
            var positionData = doc.GetElementsByTagName("PositionData").Item(0).InnerText;
            var calibAngle = doc.GetElementsByTagName("CalibrationAngle").Item(0).InnerText;
            var manipAdvanceAngle = doc.GetElementsByTagName("ManipulatorAdvanceAngle").Item(0).InnerText;
            var distBetCenters = doc.GetElementsByTagName("DistanceBetweenCenters").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("ExecutionTime", executionTime);
            ret.Add("PositionData", positionData);
            ret.Add("CalibrationAngle", calibAngle);
            ret.Add("ManipulatorAdvanceAngle", manipAdvanceAngle);
            ret.Add("DistanceBetweenCenters", distBetCenters);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseRACA : BaseResponse
    {
        public ResponseRACA(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRACA req = (ReferenceRACA)_request;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = AppConfiguration.PreAlignerResponses[_request.CommandName];
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(xmlData["CalibrationAngle"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ManipulatorAdvanceAngle"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["DistanceBetweenCenters"]);
                }
            }
            else
            {
                var preAligner = (Devices.PreAligner)device;

                _responseBuilder.Append(preAligner.BuildEOEMACA(_request));
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var calibAngle = doc.GetElementsByTagName("CalibrationAngle").Item(0).InnerText;
            var manipAdvanceAngle = doc.GetElementsByTagName("ManipulatorAdvanceAngle").Item(0).InnerText;
            var distBetCenters = doc.GetElementsByTagName("DistanceBetweenCenters").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("CalibrationAngle", calibAngle);
            ret.Add("ManipulatorAdvanceAngle", manipAdvanceAngle);
            ret.Add("DistanceBetweenCenters", distBetCenters);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

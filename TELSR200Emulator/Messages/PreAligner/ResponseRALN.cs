using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseRALN : BaseResponse
    {
        public ResponseRALN(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRALN req = (ReferenceRALN)_request;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = AppConfiguration.PreAlignerResponses[_request.CommandName];
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(xmlData["EccentircAmount"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["EccentricAngle"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["NotchDirection"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["XOffsetBefore"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["YOffsetBefore"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["PreAlignerCorrectionAngle"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ManipulatorAdjustmentAmount"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ManipulatorCorrectionAngle"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["XOffsetAfter"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["YOffestAfter"]);
                }
            }
            else
            {
                var preAligner = (Devices.PreAligner)device;
                _responseBuilder.Append(preAligner.BuildEOEMALN(_request));
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var eccAmount = doc.GetElementsByTagName("EccentircAmount").Item(0).InnerText;
            var eccAngle = doc.GetElementsByTagName("EccentricAngle").Item(0).InnerText;
            var notchDir = doc.GetElementsByTagName("NotchDirection").Item(0).InnerText;
            var xOffBefore = doc.GetElementsByTagName("XOffsetBefore").Item(0).InnerText;
            var yOffBefore = doc.GetElementsByTagName("YOffsetBefore").Item(0).InnerText;
            var preAlignCorrAngle = doc.GetElementsByTagName("PreAlignerCorrectionAngle").Item(0).InnerText;
            var ManipAdjAmt = doc.GetElementsByTagName("ManipulatorAdjustmentAmount").Item(0).InnerText;
            var ManipCorrAngle = doc.GetElementsByTagName("ManipulatorCorrectionAngle").Item(0).InnerText;
            var xOffAfter = doc.GetElementsByTagName("XOffsetAfter").Item(0).InnerText;
            var yOffAfter = doc.GetElementsByTagName("YOffestAfter").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("EccentircAmount", eccAmount);
            ret.Add("EccentricAngle", eccAngle);
            ret.Add("NotchDirection", notchDir);
            ret.Add("XOffsetBefore", xOffBefore);
            ret.Add("YOffsetBefore", yOffBefore);
            ret.Add("PreAlignerCorrectionAngle", preAlignCorrAngle);
            ret.Add("ManipulatorAdjustmentAmount", ManipAdjAmt);
            ret.Add("ManipulatorCorrectionAngle", ManipCorrAngle);
            ret.Add("XOffsetAfter", xOffAfter);
            ret.Add("YOffestAfter", yOffAfter);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseRPOS : BaseResponse
    {
        public ResponseRPOS(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRPOS req = (ReferenceRPOS)_request;

            _responseBuilder.Append(req.PositionDataType);

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(",");
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
            }
            else
            {
                _responseBuilder.Append(",");
                _responseBuilder.Append("00000200");
                _responseBuilder.Append(",");
                _responseBuilder.Append("00000200");
                _responseBuilder.Append(",");
                _responseBuilder.Append("00000200");
                _responseBuilder.Append(",");
                _responseBuilder.Append("00000200");
                _responseBuilder.Append(",");
                _responseBuilder.Append("00000200");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var positionData1 = doc.GetElementsByTagName("RotationAxis").Item(0).InnerText;
            var positionData2 = doc.GetElementsByTagName("ExtensionAxis").Item(0).InnerText;
            var positionData3 = doc.GetElementsByTagName("WristAxis1").Item(0).InnerText;
            var positionData4 = doc.GetElementsByTagName("WristAxis2").Item(0).InnerText;
            var positionData5 = doc.GetElementsByTagName("ElevationAxis").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("RotationAxis", positionData1);
            ret.Add("ExtensionAxis", positionData2);
            ret.Add("WristAxis1", positionData3);
            ret.Add("WristAxis2", positionData4);
            ret.Add("ElevationAxis", positionData5);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

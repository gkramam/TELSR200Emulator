using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
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
                var xmlData = AppConfiguration.PreAlignerResponses[_request.CommandName];
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(",");
                    _responseBuilder.Append(xmlData["PositionData"]);
                }
            }
            else
            {
                _responseBuilder.Append(",");
                _responseBuilder.Append("00000200");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var PositionData = doc.GetElementsByTagName("PositionData").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("PositionData", PositionData);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

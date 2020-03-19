using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseRERR : BaseResponse
    {
        public ResponseRERR(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRERR req = (ReferenceRERR)_request;

            _responseBuilder.Append(req.MemorySpec);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.HNo);

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ErrorCode"]);
                }
            }
            else
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append("0000");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var ErrorCode = doc.GetElementsByTagName("ErrorCode").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("ErrorCode", ErrorCode);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

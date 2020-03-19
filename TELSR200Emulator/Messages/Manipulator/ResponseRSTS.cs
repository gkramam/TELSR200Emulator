using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseRSTS : BaseResponse
    {
        public ResponseRSTS(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRSTS req = (ReferenceRSTS)_request;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = AppConfiguration.ManipulatorResponses[_request.CommandName];
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(xmlData["ErrorCode"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["Status"]);
                }
            }
            else
            {
                _responseBuilder.Append("0000");
                _responseBuilder.Append(',');
                _responseBuilder.Append("ABCD");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var ErrorCode = doc.GetElementsByTagName("ErrorCode").Item(0).InnerText;
            var Status = doc.GetElementsByTagName("Status").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("ErrorCode", ErrorCode);
            ret.Add("Status", Status);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.RLOG,MessageType.Reference,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseRLOG : BaseResponse
    {
        public ResponseRLOG(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRLOG req = (ReferenceRLOG)_request;

            _responseBuilder.Append(req.LogNumber);

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["LogData"]);
                }
            }
            else
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append("000100");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var LogData = doc.GetElementsByTagName("LogData").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("LogData", LogData);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

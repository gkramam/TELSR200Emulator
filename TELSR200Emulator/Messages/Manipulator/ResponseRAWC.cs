using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.RAWC, MessageType.Reference, CommandType.ReplyResponse, DeviceType.Manipulator)]
    public class ResponseRAWC : BaseResponse
    {
        public ResponseRAWC(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRAWC req = (ReferenceRAWC)_request;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(xmlData["XOffset"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["YOffset"]);
                }
            }
            else
            {
                _responseBuilder.Append("00000100");
                _responseBuilder.Append(',');
                _responseBuilder.Append("00000200");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var XOffset = doc.GetElementsByTagName("XOffset").Item(0).InnerText;
            var YOffset = doc.GetElementsByTagName("YOffset").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("XOffset", XOffset);
            ret.Add("YOffset", YOffset);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

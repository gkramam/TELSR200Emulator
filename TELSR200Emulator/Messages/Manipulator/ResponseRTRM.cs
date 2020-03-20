using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.RTRM,MessageType.Reference,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseRTRM : BaseResponse
    {
        public ResponseRTRM(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRTRM req = (ReferenceRTRM)_request;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(xmlData["Mode1"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["Mode2"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["Mode3"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["Mode4"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["Mode5"]);
                }
            }
            else
            {
                _responseBuilder.Append("0,0,0,0,0");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var Mode1 = doc.GetElementsByTagName("Mode1").Item(0).InnerText;
            var Mode2 = doc.GetElementsByTagName("Mode2").Item(0).InnerText;
            var Mode3 = doc.GetElementsByTagName("Mode3").Item(0).InnerText;
            var Mode4 = doc.GetElementsByTagName("Mode4").Item(0).InnerText;
            var Mode5 = doc.GetElementsByTagName("Mode5").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("Mode1", Mode1);
            ret.Add("Mode2", Mode2);
            ret.Add("Mode3", Mode3);
            ret.Add("Mode4", Mode4);
            ret.Add("Mode5", Mode5);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

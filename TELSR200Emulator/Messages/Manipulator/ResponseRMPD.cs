using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.RMPD,MessageType.Reference,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseRMPD : BaseResponse
    {
        public ResponseRMPD(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRMPD req = (ReferenceRMPD)_request;

            _responseBuilder.Append(req.TransferStation);

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["MappingResult"]);
                }
            }
            else
            {
                _responseBuilder.Append(',');
                Enumerable.Range(0, 30).ToList().ForEach(x =>
                {
                    _responseBuilder.Append($"{x.ToString("D2")}:00000000,00000000");
                    _responseBuilder.Append(',');
                });
                _responseBuilder.Remove(_responseBuilder.Length - 1, 1);
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var MappingResult = doc.GetElementsByTagName("MappingResult").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("MappingResult", MappingResult);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

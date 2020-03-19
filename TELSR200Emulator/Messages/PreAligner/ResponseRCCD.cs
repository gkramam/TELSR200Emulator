using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseRCCD : BaseResponse
    {
        public ResponseRCCD(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRCCD req = (ReferenceRCCD)_request;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = AppConfiguration.PreAlignerResponses[_request.CommandName];
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(xmlData["Light"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["CCD"]);
                }
            }
            else
            {
                _responseBuilder.Append("00100");
                _responseBuilder.Append(',');
                _responseBuilder.Append("01000");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var Light = doc.GetElementsByTagName("Light").Item(0).InnerText;
            var CCD = doc.GetElementsByTagName("CCD").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("Light", Light);
            ret.Add("CCD", CCD);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

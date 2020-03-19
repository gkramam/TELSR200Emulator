using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseRSPD : BaseResponse
    {
        public ResponseRSPD(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRSPD req = (ReferenceRSPD)_request;

            _responseBuilder.Append(req.Level);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.SpeedType);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Axis);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Axis);

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["Speed"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["MaxSpeed"]);
                }
            }
            else
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append("00000100");
                _responseBuilder.Append(',');
                _responseBuilder.Append("00001000");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var Speed = doc.GetElementsByTagName("Speed").Item(0).InnerText;
            var MaxSpeed = doc.GetElementsByTagName("MaxSpeed").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("Speed", Speed);
            ret.Add("MaxSpeed", MaxSpeed);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

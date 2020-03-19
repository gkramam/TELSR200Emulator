using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseRVER : BaseResponse
    {
        public ResponseRVER(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRVER req = (ReferenceRVER)_request;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = AppConfiguration.PreAlignerResponses[_request.CommandName];
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(xmlData["SystemSoftwareVersion"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ServoSoftwareVersion"]);
                }
            }
            else
            {
                _responseBuilder.Append("00001000");
                _responseBuilder.Append(',');
                _responseBuilder.Append("00001000");

            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var SystemSoftwareVersion = doc.GetElementsByTagName("SystemSoftwareVersion").Item(0).InnerText;
            var ServoSoftwareVersion = doc.GetElementsByTagName("ServoSoftwareVersion").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("SystemSoftwareVersion", SystemSoftwareVersion);
            ret.Add("ServoSoftwareVersion", ServoSoftwareVersion);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

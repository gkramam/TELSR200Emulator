using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseRPRM : BaseResponse
    {
        public ResponseRPRM(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRPRM req = (ReferenceRPRM)_request;

            _responseBuilder.Append(req.ParameterType);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.ParameterNumber);

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = AppConfiguration.ManipulatorResponses[_request.CommandName];
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(",");
                    _responseBuilder.Append(xmlData["ParameterValue"]);
                }
            }
            else
            {
                _responseBuilder.Append(",");
                _responseBuilder.Append("000000000200");
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var ParameterValue = doc.GetElementsByTagName("ParameterValue").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("ParameterValue", ParameterValue);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

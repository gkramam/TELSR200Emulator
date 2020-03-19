using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseRMAP : BaseResponse
    {
        public ResponseRMAP(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRMAP req = (ReferenceRMAP)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Slot);

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
                Devices.Manipulator robot = (Devices.Manipulator)device;
                _responseBuilder.Append(',');
                _responseBuilder.Append(robot.GetMappingResult(req.TransferStation, req.Slot));
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

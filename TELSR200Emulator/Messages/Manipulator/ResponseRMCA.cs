using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseRMCA : BaseResponse
    {
        public ResponseRMCA(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            ReferenceRMCA req = (ReferenceRMCA)_request;

            _responseBuilder.Append(req.TransferStation);

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                if (Convert.ToBoolean(xmlData["PositiveReply"]))
                {
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["LowestSlotPosition"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["HighestSolotPosition"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["WaferWidth"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ThresholdOfDoubleInsertion"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ThresholdOfSlantingInsertion1"]);
                    _responseBuilder.Append(',');
                    _responseBuilder.Append(xmlData["ThresholdOfSlantingInsertion2"]);
                }
            }
            else
            {
                var robot = (Devices.Manipulator)device;
                var robotConfig = AppConfiguration.environment.Manipulator;
                var station = robotConfig.Stations.Where(s => s.ID.Equals(req.TransferStation)).FirstOrDefault();

                if (station == null)
                    throw new ApplicationException($"Couldn't find station {req.TransferStation}. This may be because the station is not added in the environment file. Please check");

                _responseBuilder.Append(',');
                _responseBuilder.Append(station.GetCalibrationResult());
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var PositiveReply = doc.GetElementsByTagName("PositiveReply").Item(0).InnerText;
            var lowPosition = doc.GetElementsByTagName("LowestSlotPosition").Item(0).InnerText;
            var highPosition = doc.GetElementsByTagName("HighestSolotPosition").Item(0).InnerText;
            var waferWidth = doc.GetElementsByTagName("WaferWidth").Item(0).InnerText;
            var doubleInsertionThresh = doc.GetElementsByTagName("ThresholdOfDoubleInsertion").Item(0).InnerText;
            var slantingThresh1 = doc.GetElementsByTagName("ThresholdOfSlantingInsertion1").Item(0).InnerText;
            var slantingThresh2 = doc.GetElementsByTagName("ThresholdOfSlantingInsertion2").Item(0).InnerText;


            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("PositiveReply", PositiveReply);
            ret.Add("LowestSlotPosition", lowPosition);
            ret.Add("HighestSolotPosition", highPosition);
            ret.Add("WaferWidth", waferWidth);
            ret.Add("ThresholdOfDoubleInsertion", doubleInsertionThresh);
            ret.Add("ThresholdOfSlantingInsertion1", slantingThresh1);
            ret.Add("ThresholdOfSlantingInsertion2", slantingThresh2);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

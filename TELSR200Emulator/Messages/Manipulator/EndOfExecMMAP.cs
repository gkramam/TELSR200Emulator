using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.MMAP,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
	public class EndOfExecMMAP : BaseEndOfExec
    {
        public EndOfExecMMAP(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                _responseBuilder.Append(xmlData["ExecutionTime"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["RotationAxis"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["ExtensionAxis"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["WristAxis1"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["WristAxis2"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["ElevationAxis"]);
                _responseBuilder.Append(',');
                _responseBuilder.Append(xmlData["MappingResult"]);
            }
            else
            {
                var robot = (Devices.Manipulator)device;
                var req = (CommandMMAP)_request;
                var robotConfig = AppConfiguration.environment.Manipulator;

                _responseBuilder.Append(_executionTime.ToString("ffffff"));
                _responseBuilder.Append(',');

                Configuration.Station station = robotConfig.Stations.Where(s => s.ID.Equals(req.TransferStation)).FirstOrDefault();

                if (station == null)
                    throw new ApplicationException($"Couldn't find station {req.TransferStation}. This may be because the station is not added in the environment file. Please check");

                robot.MoveToPosition(new Devices.RobotCoordinates(station.LowestRegisteredPosition));

                _responseBuilder.Append(robot.CurrentPositionPosture.ToString());
                _responseBuilder.Append(',');

                _responseBuilder.Append(station.GetMappingStatus(req.Slot));
            }

            return base.Generate(device);
        }

        public override Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var executionTime = doc.GetElementsByTagName("ExecutionTime").Item(0).InnerText;
            var positionData1 = doc.GetElementsByTagName("RotationAxis").Item(0).InnerText;
            var positionData2 = doc.GetElementsByTagName("ExtensionAxis").Item(0).InnerText;
            var positionData3 = doc.GetElementsByTagName("WristAxis1").Item(0).InnerText;
            var positionData4 = doc.GetElementsByTagName("WristAxis2").Item(0).InnerText;
            var positionData5 = doc.GetElementsByTagName("ElevationAxis").Item(0).InnerText;
            var result = doc.GetElementsByTagName("MappingResult").Item(0).InnerText;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("ExecutionTime", executionTime);
            ret.Add("RotationAxis", positionData1);
            ret.Add("ExtensionAxis", positionData2);
            ret.Add("WristAxis1", positionData3);
            ret.Add("WristAxis2", positionData4);
            ret.Add("ElevationAxis", positionData5);
            ret.Add("MappingResult", result);
            return ret.Union(base.ReadXML(xmlDoc)).ToDictionary(k => k.Key, k => k.Value);
        }
    }
}

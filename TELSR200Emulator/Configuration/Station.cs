using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TELSR200Emulator.Configuration
{
    public class Station
    {
        public StationType Type { get; set; }
        public string ID { get; set; }
        public double WaferWidth { get; set; }

        public List<Slot> Slots { get; set; }
        public List<Threshold> Thresholds { get; set; }

        public ManipulatorPosition LowestRegisteredPosition { get; set; }
        public ManipulatorPosition HighestRegisteredPosition { get; set; }
        public ManipulatorPosition RegisteredG4Position { get; set; }
        public ManipulatorPosition RegisteredP4Position { get; set; }

        public double LowestSlotPosition { get; set; }
        public double HighestSlotPosition { get; set; }
        public Station(XmlNode stationNode)
        {
            if (stationNode == null)
            {
                throw new ApplicationException("Invalid Environment xml file. No station node found");
            }

            Type = (StationType)Enum.Parse(typeof(StationType), stationNode.Attributes["type"].Value);
            ID = stationNode.Attributes["id"].Value;

            Slots = new List<Slot>();
            foreach (XmlNode slotNode in stationNode.SelectNodes("Slot"))
            {
                Slots.Add(new Slot(slotNode));
            }

            Thresholds = new List<Threshold>();

            LowestRegisteredPosition = new ManipulatorPosition(stationNode.SelectSingleNode("Positions/Position[@key='Lowest']"));

            RegisteredG4Position = new ManipulatorPosition(stationNode.SelectSingleNode("Positions//Position[@key='G4']"));
            RegisteredP4Position = new ManipulatorPosition(stationNode.SelectSingleNode("Positions//Position[@key='P4']"));

            if (Type == StationType.Casette)
            {
                HighestRegisteredPosition = new ManipulatorPosition(stationNode.SelectSingleNode("Positions//Position[@key='Highest']"));
                WaferWidth = double.Parse(stationNode["WaferWidth"].InnerText) * 0.001;
                LowestSlotPosition = double.Parse(stationNode["LowestSlotPosition"].InnerText) * 0.001;
                HighestSlotPosition = double.Parse(stationNode["HighestSlotPosition"].InnerText) * 0.001;

                foreach (XmlNode t in stationNode.SelectSingleNode("Thresholds").ChildNodes)
                {
                    Thresholds.Add(new Threshold(t));
                }
            }
        }

        public string GetCalibrationResult()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(((int)(LowestSlotPosition / 0.001)).ToString("D8"));
            ret.Append(',');
            ret.Append(((int)(HighestSlotPosition / 0.001)).ToString("D8"));
            ret.Append(',');
            ret.Append(((int)(WaferWidth / 0.001)).ToString("D8"));
            ret.Append(',');
            ret.Append(((int)Thresholds.Find(t => t.Type == MappingCalibrationThresholdType.DoubleInsertion).Value).ToString("D8"));
            ret.Append(',');
            ret.Append(((int)Thresholds.Find(t => t.Type == MappingCalibrationThresholdType.SlantingInsertion1).Value).ToString("D8"));
            ret.Append(',');
            ret.Append(((int)Thresholds.Find(t => t.Type == MappingCalibrationThresholdType.SlantingInsertion2).Value).ToString("D8"));
            return ret.ToString();
        }

        public string GetMappingStatus(string slotIDString)
        {
            if (Type == StationType.PreAligner || Type == StationType.Transfer)
            {
                return Slots.First().GetSlotMapStatus();
            }
            else
            {
                if (slotIDString.Equals("00"))
                {
                    StringBuilder _responseBuilder = new StringBuilder();
                    bool removeComma = false;
                    foreach (var s in Slots)
                    {
                        _responseBuilder.Append(s.GetSlotMapStatus());
                        _responseBuilder.Append(',');
                        removeComma = true;
                    }
                    if (removeComma)
                        _responseBuilder.Remove(_responseBuilder.Length - 1, 1);
                    return _responseBuilder.ToString();
                }
                else
                {
                    return Slots.Where(s => s.ID.ToString("D2").Equals(slotIDString)).FirstOrDefault().GetSlotMapStatus();
                }
            }
        }
    }
    public enum StationType
    {
        Casette,
        Transfer,
        PreAligner
    }
}

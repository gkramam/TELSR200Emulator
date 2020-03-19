using System;
using System.Xml;

namespace TELSR200Emulator.Configuration
{
    public class Slot
    {
        public int ID { get; set; }
        public SlotStatus Status { get; set; }
        public Slot(XmlNode slotNode)
        {
            if (slotNode == null)
            {
                throw new ApplicationException("Invalid Environment xml file. No slot node found");
            }

            ID = Convert.ToInt32(slotNode.Attributes["id"].Value);
            Status = (SlotStatus)Enum.Parse(typeof(SlotStatus), slotNode.Attributes["status"].Value);
        }

        public string GetSlotMapStatus()
        {
            return $"{ID.ToString("D2")}:{GetMappingStatus()}";

        }
        private string GetMappingStatus()
        {
            switch (Status)
            {
                case SlotStatus.Empty:
                    return "--";
                case SlotStatus.Present:
                    return "OK";
                case SlotStatus.Inclined:
                    return "CW";
                case SlotStatus.DoubleInsertion:
                    return "DW";
                default:
                    return "--";
            }
        }
    }

    public enum SlotStatus
    {
        Empty,
        Present,
        Protruded,
        DoubleInsertion,
        Inclined
    }
}

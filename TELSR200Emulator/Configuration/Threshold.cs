using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TELSR200Emulator.Configuration
{
    public class Threshold
    {
        public double Value { get; set; }

        public MappingCalibrationThresholdType Type { get; set; }

        public Threshold(XmlNode tNode)
        {
            if (tNode == null)
            {
                throw new ApplicationException("Invalid Environment xml file. No threshold node found");
            }

            Type = (MappingCalibrationThresholdType)Enum.Parse(typeof(MappingCalibrationThresholdType), tNode.Name);
            Value = double.Parse(tNode.InnerText) * 0.001;
        }

        public override string ToString()
        {
            return ((int)Value / 0.001).ToString("D8");
        }

    }

    public enum MappingCalibrationThresholdType
    {
        DoubleInsertion,
        SlantingInsertion1,
        SlantingInsertion2
    }
}

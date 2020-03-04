using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TELSR200Emulator.Configuration
{
    public class ManipulatorPosition
    {
        public double RotationAxis { get; set; }
        public double ExtensionAxis { get; set; }
        public double WristAxis1 { get; set; }
        public double WristAxis2 { get; set; }
        public double ElevationAxis { get; set; }

        public ManipulatorPosition(XmlNode positionNode)
        {

            if (positionNode == null)
            {
                throw new ApplicationException("Invalid Environment xml file. No position node found");
            }

            RotationAxis = double.Parse(positionNode["RotationAxis"].InnerText) * 0.001;
            ExtensionAxis = double.Parse(positionNode["ExtensionAxis"].InnerText) * 0.001;
            WristAxis1 = double.Parse(positionNode["WristAxis1"].InnerText) * 0.001;
            WristAxis2 = double.Parse(positionNode["WristAxis2"].InnerText) * 0.001;
            ElevationAxis = double.Parse(positionNode["ElevationAxis"].InnerText) * 0.001;
        }
    }
}

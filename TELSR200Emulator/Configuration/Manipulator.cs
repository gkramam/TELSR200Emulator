using System;
using System.Collections.Generic;
using System.Xml;

namespace TELSR200Emulator.Configuration
{
    public class Manipulator
    {
        public ManipulatorPosition HomePosition { get; set; }

        public List<Station> Stations { get; set; }
        public Manipulator(XmlNode manipulatorNode)
        {
            if (manipulatorNode == null)
            {
                throw new ApplicationException("Invalid Environment xml file. No Manipulator node found");
            }

            HomePosition = new ManipulatorPosition(manipulatorNode.SelectSingleNode("Position[@key='Home']"));
            Stations = new List<Station>();
            foreach (XmlNode snode in manipulatorNode.SelectNodes("Stations/Station"))
            {
                Stations.Add(new Station(snode));
            }
        }
    }
}

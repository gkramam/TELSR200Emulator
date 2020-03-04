using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TELSR200Emulator.Configuration
{
    public class Environment
    {
        public Manipulator Manipulator { get; set; }
        public Prealigner Prealigner { get; set; }
        public Environment(XmlNode envNode)
        {
            if (envNode == null)
            {
                throw new ApplicationException("Invalid Environment xml file. No Environment node found");
            }

            Manipulator = new Manipulator(envNode.SelectSingleNode("//Device[@name='Manipulator']"));
            Prealigner = new Prealigner(envNode.SelectSingleNode("//Device[@name='PreAligner']"));
        }
    }
}

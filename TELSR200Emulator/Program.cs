using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Instance.Log("Starting Emulator...");

            var port1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit1Port"]);
            var port2 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit2Port"]);

            Emulation emulation = new Emulation(port1,port2);
            emulation.Start();

            Logger.Instance.Log("Exiting Emulator...");
        }
    }
}

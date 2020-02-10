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

            Emulation emulation = new Emulation();
            emulation.Start();

            Console.WriteLine("Press return to exit");
            emulation.StopEmulation = true;
            Console.ReadLine();
            Logger.Instance.Log("Exiting Emulator...");
        }
    }
}

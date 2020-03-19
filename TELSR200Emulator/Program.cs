using System;

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

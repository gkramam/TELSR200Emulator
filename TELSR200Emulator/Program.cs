using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TELSR200Emulator
{
    class Program
    {
        public static Dictionary<string, Type> MessageLookUp;

        static void Main(string[] args)
        {
            Logger.Instance.Log("Starting Emulator...");
            
            MessageLookUp = new Dictionary<string, Type>();
            foreach (Type type in typeof(Program).Assembly.GetTypes())
            {
                var attr = type.GetCustomAttributes(typeof(MessageAttribute), false).FirstOrDefault() as MessageAttribute;
                if (attr != null)
                {
                    MessageLookUp.Add($"{attr.DeviceType}{attr.CommandName}{attr.CommandType}", type);
                }
            }

            Emulation emulation = new Emulation();
            emulation.Start();

            Console.WriteLine("Press return to exit");
            emulation.StopEmulation = true;
            Console.ReadLine();
            Logger.Instance.Log("Exiting Emulator...");
        }
    }
}

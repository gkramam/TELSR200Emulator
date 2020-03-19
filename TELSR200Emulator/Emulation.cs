using System;
using System.Threading.Tasks;
using TELSR200Emulator.Devices;

namespace TELSR200Emulator
{
    public class Emulation // This is just a nice place to hold everything together logically
    {
        static public Manipulator robot;
        static public PreAligner preAligner;
        static public TcpWorker robotTcpWorker, preAlignerTcpWorker;

        public bool StopEmulation = false;

        public Emulation()
        {
            robot = new Manipulator() { IsReady = true };
            preAligner = new PreAligner() { IsReady = true };
        }

        public void Start()
        {
            robot.Start();
            preAligner.Start();

            robotTcpWorker = new TcpWorker(AppConfiguration.manipulatorPortNumber);
            //Task.Run(()=> { robotTcpWorker.Start(AddToIncomingQ); });
            Task.Run(() => { robotTcpWorker.Start(robot.AddToIncomingQ); });

            preAlignerTcpWorker = new TcpWorker(AppConfiguration.preAlignerPortNumber);
            //Task.Run(() => { preAlignerTcpWorker.Start(AddToIncomingQ); });
            Task.Run(() => { preAlignerTcpWorker.Start(preAligner.AddToIncomingQ); });
        }

        public void Stop()
        {
            StopEmulation = true;
            //robotTcpWorker.Stop = true;
            //preAlignerTcpWorker.Stop = true;
            robotTcpWorker.StopWorker();
            preAlignerTcpWorker.StopWorker();
            robot.Stop = true;
            preAligner.Stop = true;
        }
    }

    public class CommandContext
    {
        public string CommandMessage;
        public Action<string> ResponseQCallback;

        public CommandContext(Action<string> responseQCallback, string commandMessage)
        {
            CommandMessage = commandMessage;
            ResponseQCallback = responseQCallback;
        }
    }
}

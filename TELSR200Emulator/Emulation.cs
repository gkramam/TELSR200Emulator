using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TELSR200Emulator.Devices;

namespace TELSR200Emulator
{
    public class Emulation // This is just a nice place to hold everything together logically
    {
        private object _lock = new object();
        Controller controller;
        Manipulator robot;
        TcpWorker robotTcpWorker;
        ManualResetEvent isAnythingToProcess = new ManualResetEvent(false);

        Queue<CommandContext> commandQueue;
        

        public bool StopEmulation = false;

        public Emulation(){

            commandQueue = new Queue<CommandContext>();
            robot = new Manipulator();
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (!StopEmulation)
                {
                    isAnythingToProcess.WaitOne();
                    
                    //while(commandQueue.Count ==0)
                    //    Thread.Sleep(1000);

                    while (commandQueue.Count>0)
                    {
                        Process(commandQueue.Dequeue());
                        if(commandQueue.Count ==0)
                            isAnythingToProcess.Reset();
                    }

                    
                }
            });

            robotTcpWorker = new TcpWorker();
            robotTcpWorker.Start(QCommands);
        }

        public void Stop()
        {
            StopEmulation = true;
            robotTcpWorker.Stop = true;
        }
        void QCommands(CommandContext commandContext)
        {
            commandQueue.Enqueue(commandContext);
            //Thread.Sleep(3000);
            isAnythingToProcess.Set();
        }

        void Process(CommandContext cmdCxt)
        {
            var cmdstr = cmdCxt.CommandMessage;
            var checkSum = cmdstr.Substring(cmdstr.Length - 1 - 2, 2);
            string strippedCmd = cmdstr.Substring(1, cmdstr.Length - 1 - 3);
            int unit = Convert.ToInt32(cmdstr.Substring(2, 1));

            if (CheckSum.Valid(strippedCmd, checkSum))
            {
                Console.WriteLine("Checksum validation passed");
                //cmdCxt.ResponseQCallback("Checksum validation passed");
            }
            else
            {
                Console.WriteLine("Checksum validation failed");
                //cmdCxt.ResponseQCallback($"Checksum validation failed. Received {cmdstr}");
            }

            var cmdName = GetCommandName(cmdCxt.CommandMessage);

            switch(cmdName)
            {
                case "INIT":
                    robot.Process<CommandINIT, ResponseINIT,EndOfExecINIT>(cmdCxt);
                    break;
                case "ACKN":
                    robot.ProcessACKN(cmdCxt);
                    break;
            }
        }

        string GetCommandName(string message)
        {
            return message.Substring(4, 4);
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

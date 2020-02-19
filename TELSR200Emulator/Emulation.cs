using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TELSR200Emulator.Devices;
using TELSR200Emulator.Messages;
using TELSR200Emulator.Messages.PreAligner;

namespace TELSR200Emulator
{
    public class Emulation // This is just a nice place to hold everything together logically
    {
        private object _lock = new object();
        Controller controller;
        Manipulator robot;
        PreAligner preAligner;
        TcpWorker robotTcpWorker, preAlignerTcpWorker;
        ManualResetEvent isAnythingToProcess = new ManualResetEvent(false);

        ConcurrentQueue<CommandContext> commandQueue;
        

        public bool StopEmulation = false;

        public Emulation(){

            commandQueue = new ConcurrentQueue<CommandContext>();
            robot = new Manipulator();
            preAligner = new PreAligner();
        }

        public void Start()
        {
            //Task.Run(() =>
            //{
            //    while (!StopEmulation)
            //    {
            //        isAnythingToProcess.WaitOne();

            //        //while(commandQueue.Count ==0)
            //        //    Thread.Sleep(1000);
            //        CommandContext cmd = null;
            //        while (commandQueue.TryDequeue(out cmd))
            //        {
            //            if (cmd == null)
            //                continue;
            //            Task.Run(() => { Process(cmd); });
            //        }
            //        isAnythingToProcess.Reset();
                    
            //        //while (commandQueue.Count>0)
            //        //{
            //        //    Process(commandQueue.Dequeue());
            //        //    if(commandQueue.Count ==0)
            //        //        isAnythingToProcess.Reset();
            //        //}


            //    }
            //});

            robotTcpWorker = new TcpWorker(AppConfiguration.manipulatorPortNumber);
            Task.Run(()=> { robotTcpWorker.Start(QCommands); });

            preAlignerTcpWorker = new TcpWorker(AppConfiguration.preAlignerPortNumber);
            Task.Run(() => { preAlignerTcpWorker.Start(QCommands); });
        }

        public void Stop()
        {
            StopEmulation = true;
            robotTcpWorker.Stop = true;
            preAlignerTcpWorker.Stop = true;
        }
        void QCommands(CommandContext commandContext)
        {
            commandQueue.Enqueue(commandContext);
            //Thread.Sleep(3000);
            //isAnythingToProcess.Set();
            Task.Run(() => { Process(commandContext); });
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
                    if(unit == 1)
                        robot.Process<Messages.Manipulator.CommandINIT, Messages.Manipulator.ResponseINIT, Messages.Manipulator.EndOfExecGeneric>(cmdCxt,robot.ProcessGeneric,robot.BuildEOEGeneric);
                    else
                        preAligner.Process<Messages.PreAligner.CommandINIT, Messages.PreAligner.ResponseINIT, Messages.PreAligner.EndOfExecGeneric>(cmdCxt,preAligner.ProcessGeneric,preAligner.BuildEOEGeneric);
                    break;
                case "MTRS":
                    if (unit == 1)
                        robot.Process<Messages.Manipulator.CommandMTRS, Messages.Manipulator.ResponseMTRS, Messages.Manipulator.EndOfExecGeneric>(cmdCxt, robot.ProcessGeneric, robot.BuildEOEGeneric);
                    break;
                case "MPNT":
                    if (unit == 1)
                        robot.Process<Messages.Manipulator.CommandMPNT, Messages.Manipulator.ResponseMPNT, Messages.Manipulator.EndOfExecGeneric>(cmdCxt, robot.ProcessGeneric, robot.BuildEOEGeneric);
                    break;
                case "MCTR":
                    if (unit == 1)
                        robot.Process<Messages.Manipulator.CommandMCTR, Messages.Manipulator.ResponseMCTR, Messages.Manipulator.EndOfExecGeneric>(cmdCxt, robot.ProcessGeneric, robot.BuildEOEGeneric);
                    break;
                case "MTCH":
                    if (unit == 1)
                        robot.Process<Messages.Manipulator.CommandMTCH, Messages.Manipulator.ResponseMTCH, Messages.Manipulator.EndOfExecGeneric>(cmdCxt, robot.ProcessGeneric, robot.BuildEOEGeneric);
                    break;
                case "MABS":
                    if (unit == 1)
                        robot.Process<Messages.PreAligner.CommandMABS, ResponseMABS, Messages.Manipulator.EndOfExecGeneric>(cmdCxt, robot.ProcessGeneric, robot.BuildEOEGeneric);
                    else
                        preAligner.Process<Messages.PreAligner.CommandMABS, Messages.PreAligner.ResponseMABS, Messages.PreAligner.EndOfExecGeneric>(cmdCxt,preAligner.ProcessGeneric,preAligner.BuildEOEGeneric);
                    break;
                case "MREL":
                    if (unit == 1)
                        robot.Process<CommandMREL, ResponseMREL, Messages.Manipulator.EndOfExecGeneric>(cmdCxt, robot.ProcessGeneric, robot.BuildEOEGeneric);
                    else
                        preAligner.Process<Messages.PreAligner.CommandMREL, Messages.PreAligner.ResponseMREL, Messages.PreAligner.EndOfExecGeneric>(cmdCxt,preAligner.ProcessGeneric,preAligner.BuildEOEGeneric);
                    break;
                case "MMAP":
                    if (unit == 1)
                        robot.Process<Messages.Manipulator.CommandMMAP, Messages.Manipulator.ResponseMMAP, Messages.Manipulator.EndOfExecMMAP>(cmdCxt, robot.ProcessMMAP, robot.BuildEOEMMAP);
                    break;
                case "MMCA":
                    if (unit == 1)
                        robot.Process<Messages.Manipulator.CommandMMCA, Messages.Manipulator.ResponseMMCA, Messages.Manipulator.EndOfExecMMCA>(cmdCxt, robot.ProcessMMCA, robot.BuildEOEMMCA);
                    break;
                case "MALN":
                    if (unit == 2)
                        preAligner.Process<Messages.PreAligner.CommandMALN, Messages.PreAligner.ResponseMALN, Messages.PreAligner.EndOfExecMALN>(cmdCxt, preAligner.ProcessMALN, preAligner.BuildEOEMALN);
                    break;
                case "MACA":
                    if (unit == 2)
                        preAligner.Process<Messages.PreAligner.CommandMACA, Messages.PreAligner.ResponseMACA, Messages.PreAligner.EndOfExecMACA>(cmdCxt, preAligner.ProcessMACA, preAligner.BuildEOEMACA);
                    break;
                case "ACKN":
                    if (unit == 1)
                        robot.ProcessACKN(cmdCxt);
                    else
                        preAligner.ProcessACKN(cmdCxt);
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

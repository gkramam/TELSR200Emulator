using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TELSR200Emulator
{
    public class TcpConnection
    {
        private object _lock = new object();

        TcpClient innerConnection;

        ManualResetEvent isAnythingToWrite = new ManualResetEvent(false);

        Timer messageTimer; 

        public bool Stop = false;

        StringBuilder commandString = new StringBuilder();
        bool startDetected = false;

        public ConcurrentQueue<string> responseQ;
        public TcpConnection(TcpClient connection) {

            if (connection == null)
                throw new Exception("connection cannot be null");
            
            innerConnection = connection;

            responseQ = new ConcurrentQueue<string>();

            messageTimer = new Timer(AppConfiguration.tcpBetweenCharacterTimeout);
            messageTimer.Enabled = false;
            messageTimer.AutoReset = false;
            messageTimer.Elapsed += MessageTimer_Elapsed;

        }
        private void MessageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            messageTimer.Stop();
            commandString = new StringBuilder();
            startDetected = false;
        }

        public void QResponse(string message)
        {
            lock(_lock)
            {
                responseQ.Enqueue(message);
            }

            isAnythingToWrite.Set();
        }

        public void Start(Action<CommandContext> qCommandCallback)
        {
            Stream s = innerConnection.GetStream();
            StreamReader sr = new StreamReader(s, Encoding.ASCII);
            StreamWriter sw = new StreamWriter(s, Encoding.ASCII);
            sw.AutoFlush = true;

            Task.Run(() => 
            {
                if (innerConnection != null && innerConnection.Connected)
                {
                    while (!Stop && innerConnection.Connected) //Write Loop.
                    {
                        isAnythingToWrite.WaitOne();
                        //while (responseQ.Count > 0)
                        //{
                        //    var res = string.Empty;
                        //    lock (_lock)
                        //    {
                        //        res = responseQ.Dequeue();
                        //    }
                        //    sw.WriteLine(res);
                        //}
                        var res = string.Empty;
                        while (responseQ.TryDequeue(out res))
                        {
                            sw.WriteLine(res);
                        }
                        isAnythingToWrite.Reset();
                    }
                }
            });

            Task.Run(() =>
            {
                if (innerConnection != null && innerConnection.Connected)
                {
                    while (!Stop && innerConnection.Connected) //Connection Loop.
                    {
                        var amt = innerConnection.Available;

                        if (amt <= 0)
                            continue;

                        //string cmd = sr.ReadLine();

                        //while (String.IsNullOrEmpty(cmd))
                        //{
                        //    cmd = sr.ReadLine();
                        //}
                        //qCommandCallback(new CommandContext(QResponse, cmd+'\r'));


                        char read = (char)sr.Read();
                        //Console.WriteLine(read);

                        if (read == '$' && !startDetected)
                        {
                            startDetected = true;
                            messageTimer.Start();
                            commandString.Append(read);
                        }
                        else if (read == '$' && startDetected)
                        {
                            commandString = new StringBuilder();
                            messageTimer.Stop();

                            startDetected = true;
                            messageTimer.Start();
                            commandString.Append(read);
                        }
                        else if (read == '\r')
                        {
                            //Console.Write('R');
                            if (startDetected)
                            {
                                messageTimer.Stop();
                                commandString.Append(read);
                                var cmd = commandString.ToString();
                                //readCommandQ.Enqueue(cmd);
                                qCommandCallback(new CommandContext(QResponse, cmd));
                                //Task.Run(()=> { sw.WriteLine(cmd); });
                                commandString = new StringBuilder();
                                startDetected = false;
                            }
                            else
                            {
                                commandString = new StringBuilder();
                                messageTimer.Stop();
                            }
                        }
                        else
                        {
                            messageTimer.Stop();
                            commandString.Append(read);
                            messageTimer.Start();
                        }

                    }

                    if (Stop)
                    {
                        //wait until any pending data is written
                        innerConnection.Close();
                        innerConnection.Dispose();
                    }
                }
            });
        }
    }
}

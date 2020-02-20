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
        TcpClient innerConnection;

        Timer messageTimer; 

        public bool Stop = false;

        StringBuilder commandString = new StringBuilder();
        bool startDetected = false;

        BlockingCollection<string> outgoingQ;
        
        public TcpConnection(TcpClient connection) 
        {
            if (connection == null)
                throw new Exception("connection cannot be null");
            
            innerConnection = connection;

            outgoingQ = new BlockingCollection<string>(new ConcurrentQueue<string>() );

            messageTimer = new Timer(AppConfiguration.tcpBetweenCharacterTimeout);
            messageTimer.Enabled = false;
            messageTimer.AutoReset = false;
            messageTimer.Elapsed += MessageTimer_Elapsed;
        }

        private void MessageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //messageTimer.Stop();
            //commandString = new StringBuilder();
            //startDetected = false;
        }

        public void QResponse(string message)
        {
            outgoingQ.Add(message);
        }

        public void StartWriteLoop()
        {
            Stream s = innerConnection.GetStream();
            
            using(StreamWriter sw = new StreamWriter(s, Encoding.ASCII))
            {
                sw.AutoFlush = true;

                if (innerConnection != null && innerConnection.Connected && innerConnection.Client.Connected)
                {
                    while (!Stop && innerConnection.Connected && innerConnection.Client.Connected) //Write Loop.
                    {
                        foreach(var msg in outgoingQ.GetConsumingEnumerable())
                        {
                            var chars = msg.ToCharArray();
                            foreach (char c in chars)
                            {
                                sw.Write(c);
                                Thread.Sleep(10);// not working for less than 5ms
                            }
                        }
                    }
                    if (Stop)
                    {
                        innerConnection.Close();
                        innerConnection.Dispose();
                        messageTimer.Dispose();
                        outgoingQ.Dispose();
                    }
                    else
                    {
                        innerConnection.Close();
                        innerConnection.Dispose();
                        messageTimer.Dispose();
                        outgoingQ.Dispose();
                        Console.WriteLine("Writer - Connection Closed");
                    }
                }
            }
        }

        public void StartReadLoop(Action<CommandContext> qCommandCallback)
        {
            Stream s = innerConnection.GetStream();

            using (StreamReader sr = new StreamReader(s, Encoding.ASCII))
            {
                if (innerConnection != null && innerConnection.Connected)
                {
                    while (!Stop && innerConnection.Connected)
                    {
                        var amt = innerConnection.Available;

                        if (amt <= 0)
                        {
                            Thread.Sleep(1);
                            continue;
                        }
                            

                        char read = (char)sr.Read();

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
                            if (startDetected)
                            {
                                messageTimer.Stop();
                                commandString.Append(read);
                                var cmd = commandString.ToString();
                                //Task.Run(()=>qCommandCallback(new CommandContext(QResponse, cmd)));
                                qCommandCallback(new CommandContext(QResponse, cmd));
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
                }
            }
            
            if (Stop)
            {
                innerConnection.Close();
                innerConnection.Dispose();
                messageTimer.Dispose();
            }
            else
            {
                innerConnection.Close();
                innerConnection.Dispose();
                messageTimer.Dispose();
                Console.WriteLine("Reader - Connection Closed");
            }
        }

        public void Start(Action<CommandContext> qCommandCallback)
        {
            //Thread writeThread = new Thread(() => { StartWriteLoop(); });
            ////writeThread.Name = "TcpConnectionWriteLoop";
            //writeThread.Priority = ThreadPriority.AboveNormal;
            //writeThread.Start();
            Task.Run(() => { StartWriteLoop(); });

            //Thread readThread = new Thread(() => { StartReadLoop(qCommandCallback); });
            ////readThread.Name = "TcpConnectionReadLoop";
            //readThread.Priority = ThreadPriority.AboveNormal;
            //readThread.Start();
            Task.Run(() => { StartReadLoop(qCommandCallback); });
        }
    }
}

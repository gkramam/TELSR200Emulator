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

        public void StartWriteLoop()
        {
            Stream s = innerConnection.GetStream();
            
            using(StreamWriter sw = new StreamWriter(s, Encoding.ASCII))
            {
                sw.AutoFlush = true;

                if (innerConnection != null && innerConnection.Connected)
                {
                    while (!Stop && innerConnection.Connected) //Write Loop.
                    {
                        isAnythingToWrite.WaitOne();
                        var res = string.Empty;
                        while (responseQ.TryDequeue(out res))
                        {
                            var chars = res.ToCharArray();
                            foreach (char c in chars)
                            {
                                sw.Write(c);
                                Thread.Sleep(5);// Math.Max(5,AppConfiguration.tcpBetweenCharacterTimeout-5));
                            }
                        }
                        isAnythingToWrite.Reset();
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
                            continue;

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
            }
        }

        public void Start(Action<CommandContext> qCommandCallback)
        {
            Task.Run(() => { StartWriteLoop();});

            Task.Run(() =>{ StartReadLoop(qCommandCallback); });
        }
    }
}

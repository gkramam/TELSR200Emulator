﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace TELSR200Emulator
{
    public class TcpConnection : IDisposable
    {
        private bool _isDisposed;

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
            innerConnection.NoDelay = true;

            outgoingQ = new BlockingCollection<string>(new ConcurrentQueue<string>());

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

            using (StreamWriter sw = new StreamWriter(s, Encoding.ASCII))
            {
                sw.AutoFlush = true;

                if (innerConnection != null && innerConnection.Connected && innerConnection.Client.Connected)
                {
                    foreach (var msg in outgoingQ.GetConsumingEnumerable())
                    {
                        if (innerConnection.Client.Poll(-1, SelectMode.SelectWrite))
                        {
                            sw.Write(msg);
                        }
                        if (Stop)
                            break;
                    }
                    if (!Stop)
                        Console.WriteLine("Writer - Connection Closed");
                    Dispose();
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
                        if (innerConnection.Client.Poll(-1, SelectMode.SelectRead))
                        {
                            var amt = innerConnection.ReceiveBufferSize;
                            char[] readBuffer = new char[amt];
                            var length = sr.Read(readBuffer, 0, amt);
                            readBufferQ.Add(readBuffer.Take(length).ToArray());
                        }
                    }
                }
            }

            if (!Stop)
                Console.WriteLine("Reader - Connection Closed");
            Dispose();
        }

        BlockingCollection<char[]> readBufferQ = new BlockingCollection<char[]>();

        void ProcessReadBuffer(Action<CommandContext> qCommandCallback)
        {
            foreach (var buffer in readBufferQ.GetConsumingEnumerable())
            {
                foreach (var read in buffer)
                {
                    if (read == '$' && !startDetected)
                    {
                        commandString = new StringBuilder();
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
                            Console.WriteLine($"Received Request : {cmd}");
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
                        if (startDetected)
                        {
                            messageTimer.Stop();
                            commandString.Append(read);
                            messageTimer.Start();
                        }
                    }
                }
            }
        }
        public void Start(Action<CommandContext> qCommandCallback)
        {
            //Thread writeThread = new Thread(() => { StartWriteLoop(); });
            ////writeThread.Name = "TcpConnectionWriteLoop";
            //writeThread.Priority = ThreadPriority.AboveNormal;
            //writeThread.IsBackground = true;
            //writeThread.Start();
            Task.Run(() => { StartWriteLoop(); });

            //Thread readThread = new Thread(() => { StartReadLoop(qCommandCallback); });
            ////readThread.Name = "TcpConnectionReadLoop";
            //readThread.Priority = ThreadPriority.AboveNormal;
            //readThread.IsBackground = true;
            //readThread.Start();
            Task.Run(() => { StartReadLoop(qCommandCallback); });

            //Thread processThread = new Thread(() => { ProcessReadBuffer(qCommandCallback); });
            ////readThread.Name = "TcpConnectionReadLoop";
            //processThread.Priority = ThreadPriority.AboveNormal;
            //processThread.IsBackground = true;
            //processThread.Start();
            Task.Run(() => { ProcessReadBuffer(qCommandCallback); });
        }

        public void Dispose(bool disposing)
        {
            if(disposing && !_isDisposed)
            {
                var localOutGoingQ = outgoingQ;
                var localReadQ = readBufferQ;
                var localTimer = messageTimer;
                var localInnerConnection = innerConnection;
                localOutGoingQ.Dispose();
                localReadQ.Dispose();
                localTimer.Dispose();
                localInnerConnection.Dispose();
                outgoingQ = null;
                readBufferQ = null;
                messageTimer = null;
                innerConnection = null;

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);//if the class iteself doesn't have any unmanaged resources, we can skip this
        }
    }
}

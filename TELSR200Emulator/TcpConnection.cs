using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TELSR200Emulator
{
    public class TcpConnection
    {
        TcpClient innerConnection;

        Timer messageTimer; 

        public bool Stop = false;

        StringBuilder commandString = new StringBuilder();
        bool startDetected = false;

        public Queue<string> readCommandQ;
        public TcpConnection(TcpClient connection) {

            if (connection == null)
                throw new Exception("connection cannot be null");
            
            innerConnection = connection;

            readCommandQ = new Queue<string>();

            messageTimer = new Timer(1000);
            messageTimer.Enabled = false;
            messageTimer.AutoReset = false;
            messageTimer.Elapsed += MessageTimer_Elapsed;

        }
        private void MessageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            messageTimer.Stop();
            commandString.Clear();
            startDetected = false;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                if (innerConnection !=null && innerConnection.Connected)
                {
                    Stream s = innerConnection.GetStream();
                    StreamReader sr = new StreamReader(s, Encoding.ASCII);
                    StreamWriter sw = new StreamWriter(s, Encoding.ASCII);
                    sw.AutoFlush = true;

                    while (!Stop && innerConnection.Connected) //Connection Loop.
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
                            commandString.Clear();
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
                                readCommandQ.Enqueue(commandString.ToString());
                                commandString.Clear();
                                startDetected = false;
                            }
                            else
                            {
                                commandString.Clear();
                                messageTimer.Stop();
                            }
                        }
                        else
                            commandString.Append(read);

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

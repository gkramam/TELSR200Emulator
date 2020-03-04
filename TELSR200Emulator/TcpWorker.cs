using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public class TcpWorker
    {
        private readonly int _tcpWorkerLoopIdleTime;

        private TcpListener _listener;
        //private List<TcpClient> _connections;
        private List<TcpConnection> _connections;

        private TcpConnection _activeConnection;
        public TcpConnection ActiveConnection { get { return _activeConnection; } }

        private bool _started = false;

        public readonly int Port;
        bool Stop = false;

        public TcpWorker(int portNumber)
        {
            _connections = new List<TcpConnection>();
            //Stop = false;
            Port = portNumber;
            _tcpWorkerLoopIdleTime = AppConfiguration.tcpWorkerLoopIdleTime;
            _listener = TcpListener.Create(Port);
        }

        public void StopWorker()
        {
            Stop = false;
            //foreach (var c in _connections)
            //{
            //    c.Stop = true;
            //}
        }
        public void Start(Action<CommandContext> addToQCallback)
        {
            if (_started || Stop || _listener == null)
                return;
            try
            {
                _connections.Clear();// allowing for extenstion should multiple connections need be suported
                _listener.Start(Int32.MaxValue);
                _started = true;

                while (!Stop) // Listener Loop
                {
                    if (_listener.Pending())
                    {
                        var conn = _listener.AcceptTcpClient();
                        if (conn != null)
                        {
                            var tcpconn = new TcpConnection(conn);
                            _connections.Add(tcpconn);
                            _activeConnection = tcpconn;
                            tcpconn.Start(addToQCallback);
                        }
                    }

                    Thread.Sleep(_tcpWorkerLoopIdleTime);
                }

                foreach (var c in _connections)
                {
                    c.Stop = true;
                }
            }
            catch (SocketException ex)
            {
                Logger.Instance.Log(ex.InnerException);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex.InnerException);
                throw;
            }
        }
    }
}

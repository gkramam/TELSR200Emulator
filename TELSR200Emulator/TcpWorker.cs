using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public class TcpWorker
    {
        private readonly int _tcpWorkerLoopIdleTime;

        private TcpListener _listener;
        //private List<TcpClient> _connections;
        private List<TcpConnection> _connections;

        private bool _started = false;

        public readonly int Port;
        public bool Stop = false;

        public TcpWorker(int port, int tcpWorkerLoopIdleTime)
        {
            _connections = new List<TcpConnection>();
            Stop = false;
            Port = port;
            _tcpWorkerLoopIdleTime = tcpWorkerLoopIdleTime;
            _listener = TcpListener.Create(Port);
        }

        public async Task Start()
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
                            tcpconn.Start();
                        }
                    }

                    await Task.Delay(_tcpWorkerLoopIdleTime);
                }

                foreach(var c in _connections)
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

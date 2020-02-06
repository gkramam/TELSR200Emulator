using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Devices;

namespace TELSR200Emulator
{
    public class Emulation // This is just a nice place to hold everything together logically
    {
        int manipulatorPortNumber, preAlignerPortNumber, tcpWorkerLoopIdleTime;

        Controller controller;
        Manipulator robot;
        TcpWorker robotTcpWorker;

        public bool StopEmulation = false;

        public Emulation(){//(int unit1PortNumber,int unit2PortNumber){

        }

        public void LoadAppConfig()
        {
            var manipulatorPortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit1Port"]);
            var preAlignerPortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["unit2Port"]);
            var tcpWorkerLoopIdleTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["tcpWorkerLoopIdleTime"]);
        }

        public void Start()
        {
            robotTcpWorker = new TcpWorker(manipulatorPortNumber, tcpWorkerLoopIdleTime);
            robotTcpWorker.Start();
        }
    }
}

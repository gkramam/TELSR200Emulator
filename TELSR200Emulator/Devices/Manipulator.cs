using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Devices
{
    public class Manipulator
    {
        private readonly int _port;
        public Manipulator(int port)
        {
            _port = port;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public class CommandINIT: BaseMessage
    {
        private bool _errorClear;
        private bool _servoON;
        private string _homeAxes;

        public string Axis
        {
            get { return _homeAxes; }
        }
        public CommandINIT(string message):base(message)
        {

        }

        override public void Parse()
        {
            base.Parse();

            if (_fields[2] == "1")
                _errorClear = true;
            if (_fields[3] == "1")
                _servoON = true;
            _homeAxes = _fields[4];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class CommandINIT : BaseMessage
    {
        private bool _errorClear;
        private bool _servoON;
        private string _homeAxes;

        public string Axis
        {
            get { return _homeAxes; }
        }
        public CommandINIT(string message) : base(message)
        {

        }

        override public void Parse()
        {
            base.Parse();

            if (_fields[_commandNameIndex + 1] == "1")
                _errorClear = true;
            if (_fields[_commandNameIndex + 2] == "1")
                _servoON = true;
            _homeAxes = _fields[_commandNameIndex + 3];
        }
    }
}

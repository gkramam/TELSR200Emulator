using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMABS:BaseMessage
    {
        public string Axis,Hand, Mode, Value;
        public CommandMABS(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            Axis = _fields[2];
            Hand = _fields[3];
            Mode = _fields[4];
            Value = _fields[5];
        }
    }
}

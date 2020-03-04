using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class CommandMALN:BaseMessage
    {
        public string Mode, Angle;
        public CommandMALN(string msg) :base(msg)
        { }

        public override void Parse()
        {
            base.Parse();

            Mode = _fields[_commandNameIndex + 1];
            Angle = _fields[_commandNameIndex + 2];
        }

        
    }
}

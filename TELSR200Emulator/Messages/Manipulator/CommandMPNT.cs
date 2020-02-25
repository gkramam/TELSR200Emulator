using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMPNT: BaseMessage
    {
        public string TransferPoint;
        public CommandMPNT(string msg) :base(msg)
        {

        }

        public override void Parse()
        {
            base.Parse();

            TransferPoint = _fields[_commandNameIndex +1];
        }
    }
}

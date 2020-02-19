using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMMAP: BaseMessage
    {
        public string TransferStation, Slot, Posture, Safe;
        public CommandMMAP(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            TransferStation = _fields[2];
            Slot = _fields[3];
            Posture = _fields[4];
            Safe = _fields[5];
        }
    }
}

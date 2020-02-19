using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMMCA:BaseMessage
    {
        public string TransferStation, Posture, Safe;
        public CommandMMCA(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            TransferStation = _fields[2];
            Posture = _fields[3];
            Safe = _fields[4];
        }
    }
}

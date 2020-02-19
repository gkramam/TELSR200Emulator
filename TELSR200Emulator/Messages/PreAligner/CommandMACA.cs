using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class CommandMACA:BaseMessage
    {
        public string Mode;
        public CommandMACA(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            Mode = _fields[2];
        }
    }
}

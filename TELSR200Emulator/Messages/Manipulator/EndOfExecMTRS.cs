using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecMTRS : EndOfExecGeneric
    {
        public EndOfExecMTRS(BaseMessage req) : base(req) { }
    }
}

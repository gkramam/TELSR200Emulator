using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecCSRV: EndOfExecGeneric
    {
        public EndOfExecCSRV(BaseMessage req) : base(req) { }
    }
}

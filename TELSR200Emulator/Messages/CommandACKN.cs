using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages
{
    public class CommandACKN : BaseMessage
    {
        public CommandACKN(string message) : base(message) { }
    }
}

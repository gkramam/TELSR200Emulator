using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages
{
    public class ErrorEvent:BaseEvent
    {
        public override string Generate(int device, string errorCode,string result)
        {
            _builder = new StringBuilder();
            _builder.Append($"100,Error notification event,{errorCode}");
            return base.Generate(device, errorCode);
        }
    }
}

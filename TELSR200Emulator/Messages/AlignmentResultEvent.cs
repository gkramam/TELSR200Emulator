using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages
{
    public class AlignmentResultEvent:BaseEvent
    {
        public override string Generate(int device, string errorCode, string alignmentResult)
        {
            _builder = new StringBuilder();

            _builder.Append($"140,Alignment result event,{errorCode},{alignmentResult}");
            return base.Generate(device, errorCode);
        }
    }
}

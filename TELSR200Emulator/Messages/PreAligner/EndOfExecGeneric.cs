using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class EndOfExecGeneric : BaseEndOfExec
    {
        //TimeSpan _executionTime;

        public EndOfExecGeneric(BaseMessage request) : base(request)
        {
            //_executionTime = TimeSpan.FromMilliseconds(33);//33 mills
        }

        public override string Generate(Device device)
        {
            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');
            //_responseBuilder.Append("00000000");//pos1
            _responseBuilder.Append(((Devices.PreAligner)device).BuildEOEGeneric(_request));

            return base.Generate(device);
        }
    }
}

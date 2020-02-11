using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public class EndOfExecINIT: BaseEndOfExec
    {
        TimeSpan _executionTime;

        public EndOfExecINIT(BaseMessage request) : base(request) 
        {
            _executionTime = TimeSpan.FromMilliseconds(33);//33 mills
        }

        public override string Generate()
        {
            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');
            _responseBuilder.Append("00000100");//pos1
            _responseBuilder.Append(',');
            _responseBuilder.Append("00000100");//pos2
            _responseBuilder.Append(',');
            _responseBuilder.Append("00000100");//pos3
            _responseBuilder.Append(',');
            _responseBuilder.Append("00000100");//pos4
            _responseBuilder.Append(',');
            _responseBuilder.Append("00000100");//pos5

            return base.Generate();
        }
    }
}

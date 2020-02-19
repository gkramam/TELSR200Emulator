using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages
{
    public class BaseEndOfExec : BaseResponse
    {
        protected  TimeSpan _executionTime;
        public BaseEndOfExec(BaseMessage req) : base(req) 
        {
            _executionTime = TimeSpan.FromMilliseconds(33);//33 mills
        }

        public virtual string Generate(Action deviceProcessingCallback, Func<BaseMessage,string> builderCallback)
        {
            deviceProcessingCallback();

            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');

            _responseBuilder.Append(builderCallback(_request));

            return base.Generate();

        }
    }
}
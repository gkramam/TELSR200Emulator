using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Messages.Manipulator;
using TELSR200Emulator.Messages.PreAligner;

namespace TELSR200Emulator.Messages
{
    abstract public class BaseResponse
    {
        protected BaseMessage _request;

        protected string _commandName;

        protected StringBuilder _responseBuilder;
        public BaseResponse(BaseMessage request)
        {
            _request = request;
            _responseBuilder = new StringBuilder();
        }

        public virtual string Generate(Device device)
        {
            StringBuilder temp = new StringBuilder();

            temp.Append(',');
            temp.Append(_request.UnitNumber);
            temp.Append(',');
            
            if(AppConfiguration.useSequenceNumber)
            {
                temp.Append(_request.SeqNum.Value.ToString("D2"));
                temp.Append(',');
            }
            
            temp.Append(ResponseStatusCalculator.Calculate((byte)device.GetResponseStatus1()));
            temp.Append(ResponseStatusCalculator.Calculate((byte)device.GetResponseStatus2()));

            temp.Append(',');
            temp.Append("0000");//ACkCD & Errcd are same
            temp.Append(',');
            temp.Append(_request.CommandName);
            temp.Append(',');

            _responseBuilder.Insert(0, temp.ToString());

            if (AppConfiguration.checkSumCheck)
            {
                var chksum = CheckSum.Compute(_responseBuilder.ToString());
                _responseBuilder.Append(',');
                _responseBuilder.Append(chksum);
            }

            _responseBuilder.Append('\r');

            if (this is BaseEndOfExec)
                _responseBuilder.Insert(0, '!');
            else
                _responseBuilder.Insert(0, '$');

            return _responseBuilder.ToString();
        }
    }
}

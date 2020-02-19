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
        //protected int _unitNumber;
        protected ResponseSts1 _sts1;
        protected ResponseSts2 _sts2;

        protected StringBuilder _responseBuilder;
        public BaseResponse(BaseMessage request)
        {
            _request = request;
            _sts1 = ResponseSts1.ServoOff | ResponseSts1.ErrorOccured;
            _sts2 = ResponseSts2.Blade1_Vac_Grip_HasWafer | ResponseSts2.Blade2_LineSensor_Haswafer;
            _responseBuilder = new StringBuilder();
        }

        public virtual string Generate()
        {
            StringBuilder temp = new StringBuilder();

            temp.Append(',');
            temp.Append(_request.UnitNumber);
            temp.Append(',');
            
            if(AppConfiguration.useSequenceNumber)
            {
                temp.Append(_request.SeqNum.Value.ToString("X2"));
                temp.Append(',');
            }
            
            temp.Append(ResponseStatusCalculator.Calculate((byte)_sts1));
            temp.Append(ResponseStatusCalculator.Calculate((byte)_sts2));
            temp.Append(',');
            temp.Append("0000");//ACkCD & Errcd are same
            temp.Append(',');
            temp.Append(_request.CommandName);
            temp.Append(',');

            _responseBuilder.Insert(0, temp.ToString());

            var chksum = CheckSum.Compute(_responseBuilder.ToString());
            _responseBuilder.Append(',');
            _responseBuilder.Append(chksum);
            _responseBuilder.Append('\r');

            if (this is BaseEndOfExec)
                _responseBuilder.Insert(0, '!');
            else
                _responseBuilder.Insert(0, '$');

            return _responseBuilder.ToString();
        }

    }
}

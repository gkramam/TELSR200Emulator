using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseINIT : BaseResponse
    {
        public ResponseINIT(CommandINIT request) : base(request)
        {
            _sts1 = ResponseSts1.UnitReady;
            _sts2 = ResponseSts2.None;
            _request = request;
        }

        public override string Generate()
        {
            _responseBuilder = new StringBuilder();
            //response.Append(',');
            //response.Append(_unitNumber);
            //response.Append(',');
            //response.Append(ResponseStatusCalculator.Calculate((byte)_sts1));
            //response.Append(ResponseStatusCalculator.Calculate((byte)_sts2));
            //response.Append(',');
            //response.Append("0000");
            //response.Append(',');
            //response.Append(_commandName);
            //response.Append(',');
            CommandINIT req = (CommandINIT)_request;
            _responseBuilder.Append(req.ErrorClear ? "1" : "0");
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.ServoOn ? "1" : "0");
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.HomeAxis);

            return base.Generate();

            //var chksum = CheckSum.Compute(response.ToString());
            //response.Append(',');
            //response.Append(chksum);
            //response.Append('\r');
            //response.Insert(0, '$');
            //return response.ToString();
        }
    }
}

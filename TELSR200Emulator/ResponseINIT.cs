using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public class ResponseINIT: BaseResponse
    {
        //private CommandINIT _request;

        private string _commandName = "INIT";
        private int _unitNumber;
        private ResponseSts1 _sts1;
        private ResponseSts2 _sts2;
        private string _ackCd;
        private bool _errorClear;
        private bool _servoON;
        private string _axis;
        public ResponseINIT(CommandINIT request):base(request)
        {
            _sts1 = ResponseSts1.ServoOff | ResponseSts1.ErrorOccured;
            _sts2 = ResponseSts2.Blade1_Vac_Grip_HasWafer | ResponseSts2.Blade2_LineSensor_Haswafer;
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
            _responseBuilder.Append(_errorClear ? "1" : "0");
            _responseBuilder.Append(',');
            _responseBuilder.Append(_servoON ? "1" : "0");
            _responseBuilder.Append(',');
            _responseBuilder.Append(((CommandINIT)_request).Axis);

            return base.Generate();

            //var chksum = CheckSum.Compute(response.ToString());
            //response.Append(',');
            //response.Append(chksum);
            //response.Append('\r');
            //response.Insert(0, '$');
            //return response.ToString();
        }
    }

    public class ResponseStatusCalculator
    {
        public static string Calculate(byte status)
        {
            //var stsStr = status.ToString("X");
            var hexsts1 = BitConverter.ToString(new byte[] { (byte)status });//Getting the Hex Value
            var asciiSts1 = ASCIIEncoding.ASCII.GetBytes(hexsts1.ToCharArray(), 1, 1);
            return ASCIIEncoding.ASCII.GetString(asciiSts1);
        }
    }
}

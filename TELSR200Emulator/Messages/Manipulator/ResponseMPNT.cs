using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseMPNT: BaseResponse
    {
        public ResponseMPNT(CommandMPNT req) : base(req)
        {
            _sts1 = ResponseSts1.UnitReady;
            _sts2 = ResponseSts2.Blade1_Vac_Grip_HasWafer;
            _request = req;
        }

        public override string Generate()
        {
            _responseBuilder = new StringBuilder();

            CommandMPNT req = (CommandMPNT)_request;

            _responseBuilder.Append(req.TransferPoint);

            return base.Generate();
        }
    }
}

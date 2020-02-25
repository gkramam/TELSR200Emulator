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
            _request = req;
        }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMPNT req = (CommandMPNT)_request;

            _responseBuilder.Append(req.TransferPoint);

            return base.Generate(device);
        }
    }
}

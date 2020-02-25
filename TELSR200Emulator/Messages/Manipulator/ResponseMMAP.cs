using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseMMAP:BaseResponse
    {
        public ResponseMMAP(CommandMMAP req):base(req)
        { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMMAP req = (CommandMMAP)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Slot);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Safe);

            return base.Generate(device);
        }
    }
}

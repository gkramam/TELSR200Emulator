using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseMABS: BaseResponse
    {
        public ResponseMABS(CommandMABS req):base(req)
        { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMABS req = (CommandMABS)_request;

            _responseBuilder.Append(req.Axis);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Mode);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Value);

            return base.Generate(device);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseMCTR:BaseResponse
    {
        public ResponseMCTR(CommandMCTR req) : base(req)
        {
        }

        public override string Generate()
        {
            _responseBuilder = new StringBuilder();

            CommandMCTR req = (CommandMCTR)_request;

            _responseBuilder.Append(req.MotionMode);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Slot);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferPoint);

            if (req.OffsetSpecified)
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetX);
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetY);
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetZ);
            }

            if (req.AngleSpecified)
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.Angle);
            }

            return base.Generate();
        }
    }
}

using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseMTCH: BaseResponse
    {
        public ResponseMTCH(CommandMTCH req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMTCH req = (CommandMTCH)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Slot);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.PMode);

            if (req.OffsetSpecified)
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetX);
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetY);
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetZ);
            }

            return base.Generate(device);
        }
    }
}

using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.MTRS, MessageType.Action, CommandType.ReplyResponse, DeviceType.Manipulator)]
    public class ResponseMTRS : BaseResponse
    {
        public ResponseMTRS(CommandMTRS request) : base(request)
        {
            _request = request;
        }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMTRS req = (CommandMTRS)_request;

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

            return base.Generate(device);
        }
    }
}

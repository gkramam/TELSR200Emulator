using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.MMCA, MessageType.Action, CommandType.ReplyResponse, DeviceType.Manipulator)]
    public class ResponseMMCA : BaseResponse
    {
        public ResponseMMCA(CommandMMCA req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMMCA req = (CommandMMCA)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Safe);

            return base.Generate(device);
        }
    }
}

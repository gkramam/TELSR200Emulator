using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.MALN,MessageType.Action,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseMALN : BaseResponse
    {
        public ResponseMALN(CommandMALN req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMALN req = (CommandMALN)_request;

            _responseBuilder.Append(req.Mode);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Angle);

            return base.Generate(device);
        }
    }
}

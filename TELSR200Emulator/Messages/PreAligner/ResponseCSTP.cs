using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CSTP,MessageType.Control,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseCSTP : BaseResponse
    {
        public ResponseCSTP(BaseMessage req) : base(req)
        {

        }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandCSTP req = (CommandCSTP)_request;

            _responseBuilder.Append(req.StopMode);

            return base.Generate(device);
        }
    }
}

using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CSRV,MessageType.Control,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseCSRV : BaseResponse
    {
        public ResponseCSRV(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandCSRV req = (CommandCSRV)_request;

            _responseBuilder.Append(req.ServoCommand);

            return base.Generate(device);
        }
    }
}

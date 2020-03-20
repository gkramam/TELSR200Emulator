using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CSOL,MessageType.Control,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseCSOL : BaseResponse
    {
        public ResponseCSOL(BaseMessage req) : base(req) { }
        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandCSOL req = (CommandCSOL)_request;

            _responseBuilder.Append(req.SolenoidControlSpec);
            _responseBuilder.Append(req.SolenoidCommand);
            _responseBuilder.Append(req.ShouldWait ? "1" : "0");

            return base.Generate(device);
        }
    }
}

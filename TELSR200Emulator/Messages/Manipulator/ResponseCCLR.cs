using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.CCLR,MessageType.Control,CommandType.ReplyResponse,DeviceType.Manipulator)]
    public class ResponseCCLR : BaseResponse
    {
        public ResponseCCLR(BaseMessage req) : base(req) { }
        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandCCLR req = (CommandCCLR)_request;

            _responseBuilder.Append(req.CMode);

            return base.Generate(device);
        }
    }
}

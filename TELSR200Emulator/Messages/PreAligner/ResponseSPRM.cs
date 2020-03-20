using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.SPRM,MessageType.Setting,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseSPRM : BaseResponse
    {
        public ResponseSPRM(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSPRM req = (CommandSPRM)_request;

            _responseBuilder.Append(req.ParameterType);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.ParameterNumber);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Value);

            return base.Generate(device);
        }
    }
}

using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.SPLD,MessageType.Setting,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseSPLD : BaseResponse
    {
        public ResponseSPLD(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSPLD req = (CommandSPLD)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);

            return base.Generate(device);
        }
    }
}

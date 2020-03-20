using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.SSTR,MessageType.Setting,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseSSTR : BaseResponse
    {
        public ResponseSSTR(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSSTR req = (CommandSSTR)_request;

            _responseBuilder.Append(req.MemorySpec);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Item);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Value);

            return base.Generate(device);
        }
    }
}

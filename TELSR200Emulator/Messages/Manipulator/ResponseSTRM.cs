using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.STRM,MessageType.Setting,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseSTRM : BaseResponse
    {
        public ResponseSTRM(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSTRM req = (CommandSTRM)_request;

            _responseBuilder.Append(req.Mode1);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Mode2);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Mode3);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Mode4);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Mode5);

            return base.Generate(device);
        }
    }
}

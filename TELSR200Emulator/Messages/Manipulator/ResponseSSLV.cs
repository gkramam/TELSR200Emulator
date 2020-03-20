using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.SSLV,MessageType.Setting,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseSSLV : BaseResponse
    {
        public ResponseSSLV(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSSLV req = (CommandSSLV)_request;

            _responseBuilder.Append(req.Level);

            return base.Generate(device);
        }
    }
}

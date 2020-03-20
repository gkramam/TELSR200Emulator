using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.SSTD,MessageType.Setting,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseSSTD : BaseResponse
    {
        public ResponseSSTD(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSSTD req = (CommandSSTD)_request;

            _responseBuilder.Append(req.Axis);

            return base.Generate(device);
        }
    }
}

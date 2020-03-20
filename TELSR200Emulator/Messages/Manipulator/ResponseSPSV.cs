using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.SPSV,MessageType.Setting,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseSPSV : BaseResponse
    {
        public ResponseSPSV(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSPSV req = (CommandSPSV)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);

            return base.Generate(device);
        }
    }
}

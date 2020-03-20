using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.SAPS,MessageType.Setting,CommandType.ReplyResponse,DeviceType.Manipulator)]
	public class ResponseSAPS : BaseResponse
    {
        public ResponseSAPS(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSAPS req = (CommandSAPS)_request;

            _responseBuilder.Append(req.MemorySpec);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.RegistrationMode);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.OffsetX);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.OffsetY);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.OffsetZ);

            return base.Generate(device);
        }
    }
}

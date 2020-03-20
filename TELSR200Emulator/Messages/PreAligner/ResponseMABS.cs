using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.MABS,MessageType.Action,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseMABS : BaseResponse
    {
        public ResponseMABS(CommandMABS req) : base(req)
        { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMABS req = (CommandMABS)_request;

            _responseBuilder.Append(req.Axis);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Mode);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Value);

            return base.Generate(device);
        }
    }
}

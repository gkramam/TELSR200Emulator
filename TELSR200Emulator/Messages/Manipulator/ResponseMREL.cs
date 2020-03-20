using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.MREL, MessageType.Action, CommandType.ReplyResponse, DeviceType.Manipulator)]
    public class ResponseMREL : BaseResponse
    {
        public ResponseMREL(CommandMREL req) : base(req)
        {

        }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMREL req = (CommandMREL)_request;

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

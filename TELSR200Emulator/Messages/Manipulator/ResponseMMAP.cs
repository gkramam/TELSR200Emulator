using System.Text;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.MMAP, MessageType.Action, CommandType.ReplyResponse, DeviceType.Manipulator)]
    public class ResponseMMAP : BaseResponse
    {
        public ResponseMMAP(CommandMMAP req) : base(req)
        { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandMMAP req = (CommandMMAP)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Slot);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Safe);

            return base.Generate(device);
        }
    }
}

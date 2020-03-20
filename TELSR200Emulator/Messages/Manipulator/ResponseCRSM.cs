namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.CRSM, MessageType.Control, CommandType.ReplyResponse, DeviceType.Manipulator)]
    public class ResponseCRSM : BaseResponse
    {
        public ResponseCRSM(BaseMessage req) : base(req) { }
    }
}

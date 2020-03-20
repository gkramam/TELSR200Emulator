namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CRSM,MessageType.Control,CommandType.ReplyResponse,DeviceType.PreAligner)]
	public class ResponseCRSM : BaseResponse
    {
        public ResponseCRSM(BaseMessage req) : base(req) { }
    }
}

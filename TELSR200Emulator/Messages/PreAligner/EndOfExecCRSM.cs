namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CRSM,MessageType.Control,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecCRSM : EndOfExecGeneric
    {
        public EndOfExecCRSM(BaseMessage req) : base(req) { }
    }
}

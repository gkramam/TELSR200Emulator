namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CCLR,MessageType.Control,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecCCLR : EndOfExecGeneric
    {
        public EndOfExecCCLR(BaseMessage req) : base(req) { }
    }
}

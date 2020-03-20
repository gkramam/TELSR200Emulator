namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CSOL,MessageType.Control,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecCSOL : EndOfExecGeneric
    {
        public EndOfExecCSOL(BaseMessage req) : base(req) { }
    }
}

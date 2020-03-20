namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CSRV,MessageType.Control,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecCSRV : EndOfExecGeneric
    {
        public EndOfExecCSRV(BaseMessage req) : base(req) { }
    }
}

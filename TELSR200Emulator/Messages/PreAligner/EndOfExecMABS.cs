namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.MABS,MessageType.Action,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecMABS : EndOfExecGeneric
    {
        public EndOfExecMABS(BaseMessage req) : base(req) { }
    }
}

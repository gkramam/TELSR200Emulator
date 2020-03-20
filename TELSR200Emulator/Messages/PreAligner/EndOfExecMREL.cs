namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.MREL,MessageType.Action,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecMREL : EndOfExecGeneric
    {
        public EndOfExecMREL(BaseMessage req) : base(req) { }
    }
}

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.INIT,MessageType.Action,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecINIT : EndOfExecGeneric
    {
        public EndOfExecINIT(BaseMessage req) : base(req) { }
    }
}

namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.MREL,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecMREL : EndOfExecGeneric
    {
        public EndOfExecMREL(BaseMessage req) : base(req) { }
    }
}

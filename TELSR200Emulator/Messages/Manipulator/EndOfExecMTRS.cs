namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.MTRS,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecMTRS : EndOfExecGeneric
    {
        public EndOfExecMTRS(BaseMessage req) : base(req) { }
    }
}

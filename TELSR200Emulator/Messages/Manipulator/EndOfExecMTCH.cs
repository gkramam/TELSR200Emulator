namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.MTCH,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecMTCH : EndOfExecGeneric
    {
        public EndOfExecMTCH(BaseMessage req) : base(req) { }
    }
}

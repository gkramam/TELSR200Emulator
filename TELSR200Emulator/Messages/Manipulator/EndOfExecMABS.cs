namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.MABS,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecMABS : EndOfExecGeneric
    {
        public EndOfExecMABS(BaseMessage req) : base(req) { }
    }
}

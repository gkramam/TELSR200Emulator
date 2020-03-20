namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.INIT,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecINIT : EndOfExecGeneric
    {
        public EndOfExecINIT(BaseMessage req) : base(req) { }

    }
}

namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.MCTR,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecMCTR : EndOfExecGeneric
    {
        public EndOfExecMCTR(BaseMessage req) : base(req) { }
    }
}

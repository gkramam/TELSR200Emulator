namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.MPNT,MessageType.Action,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecMPNT : EndOfExecGeneric
    {
        public EndOfExecMPNT(BaseMessage req) : base(req) { }
    }
}

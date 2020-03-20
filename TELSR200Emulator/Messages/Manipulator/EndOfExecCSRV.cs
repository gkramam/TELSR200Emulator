namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.CSRV,MessageType.Control,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecCSRV : EndOfExecGeneric
    {
        public EndOfExecCSRV(BaseMessage req) : base(req) { }
    }
}

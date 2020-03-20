namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.CSOL,MessageType.Control,CommandType.ReplyEoE,DeviceType.Manipulator)]
	public class EndOfExecCSOL : EndOfExecGeneric
    {
        public EndOfExecCSOL(BaseMessage req) : base(req) { }
    }
}

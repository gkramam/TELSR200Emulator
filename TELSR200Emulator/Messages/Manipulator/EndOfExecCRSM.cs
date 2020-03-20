namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.CRSM,MessageType.Control,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecCRSM : EndOfExecGeneric
    {
        public EndOfExecCRSM(BaseMessage req) : base(req) { }
    }
}

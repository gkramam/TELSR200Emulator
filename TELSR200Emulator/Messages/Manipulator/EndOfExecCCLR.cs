namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(CommandName.CCLR,MessageType.Control,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecCCLR : EndOfExecGeneric
    {
        public EndOfExecCCLR(BaseMessage req) : base(req) { }
    }
}

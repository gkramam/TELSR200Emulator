namespace TELSR200Emulator.Messages.Manipulator
{
	[Message(CommandName.CSTP,MessageType.Control,CommandType.ReplyEoE,DeviceType.Manipulator)]
    public class EndOfExecCSTP : EndOfExecGeneric
    {
        public EndOfExecCSTP(BaseMessage req) : base(req)
        {

        }
    }
}

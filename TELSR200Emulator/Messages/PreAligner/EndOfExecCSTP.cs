namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(CommandName.CSTP,MessageType.Control,CommandType.ReplyEoE,DeviceType.PreAligner)]
	public class EndOfExecCSTP : EndOfExecGeneric
    {
        public EndOfExecCSTP(BaseMessage req) : base(req)
        {

        }
    }
}

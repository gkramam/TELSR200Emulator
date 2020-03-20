namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.CRSM,MessageType.Control,CommandType.Request,DeviceType.PreAligner)]
	public class CommandCRSM : BaseMessage
    {
        public CommandCRSM(string msg) : base(msg) { }

    }
}

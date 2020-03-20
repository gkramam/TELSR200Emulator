namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RVER,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
	public class ReferenceRVER : BaseMessage
    {
        public ReferenceRVER(string msg) : base(msg) { }
    }
}

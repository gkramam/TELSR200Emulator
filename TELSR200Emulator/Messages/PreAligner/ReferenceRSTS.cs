namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RSTS,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
	public class ReferenceRSTS : BaseMessage
    {
        public string TransferStation, Slot;
        public ReferenceRSTS(string msg) : base(msg) { }

    }
}

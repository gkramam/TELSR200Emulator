namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RSTS,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRSTS : BaseMessage
    {
        public string TransferStation, Slot;
        public ReferenceRSTS(string msg) : base(msg) { }

    }
}

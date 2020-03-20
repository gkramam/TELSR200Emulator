namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RCCD,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
	public class ReferenceRCCD : BaseMessage
    {
        public ReferenceRCCD(string msg) : base(msg) { }

    }
}

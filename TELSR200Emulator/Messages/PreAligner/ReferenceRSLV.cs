namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RSLV,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
	public class ReferenceRSLV : BaseMessage
    {
        public ReferenceRSLV(string msg) : base(msg) { }
    }
}

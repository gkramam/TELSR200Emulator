namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RMSK,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
	public class ReferenceRMSK : BaseMessage
    {
        public ReferenceRMSK(string msg) : base(msg) { }
    }
}

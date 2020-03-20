namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RALN,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
	public class ReferenceRALN : BaseMessage
    {
        public ReferenceRALN(string msg) : base(msg) { }

    }
}

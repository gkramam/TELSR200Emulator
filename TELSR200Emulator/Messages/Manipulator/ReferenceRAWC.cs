namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RAWC,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRAWC : BaseMessage
    {
        public ReferenceRAWC(string msg) : base(msg) { }
    }
}

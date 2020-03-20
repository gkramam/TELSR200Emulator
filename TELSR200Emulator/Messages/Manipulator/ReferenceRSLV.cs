namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RSLV,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRSLV : BaseMessage
    {
        public ReferenceRSLV(string msg) : base(msg) { }
    }
}

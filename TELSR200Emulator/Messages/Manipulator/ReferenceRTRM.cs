namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RTRM,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRTRM : BaseMessage
    {
        public ReferenceRTRM(string msg) : base(msg) { }
    }
}

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RVER,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRVER : BaseMessage
    {
        public ReferenceRVER(string msg) : base(msg) { }
    }
}

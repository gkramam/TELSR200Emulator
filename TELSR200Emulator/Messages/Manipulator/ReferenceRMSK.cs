namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RMSK,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRMSK : BaseMessage
    {
        public ReferenceRMSK(string msg) : base(msg) { }
    }
}

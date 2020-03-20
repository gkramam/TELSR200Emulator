namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.CRSM,MessageType.Control,CommandType.Request,DeviceType.Manipulator)]
	public class CommandCRSM : BaseMessage
    {
        public CommandCRSM(string msg) : base(msg) { }

    }
}

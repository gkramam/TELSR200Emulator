namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.CSTP,MessageType.Control,CommandType.Request,DeviceType.Manipulator)]
	public class CommandCSTP : BaseMessage
    {
        public string StopMode;
        public CommandCSTP(string msg) : base(msg)
        {

        }

        public override void Parse()
        {
            base.Parse();
            StopMode = _fields[_commandNameIndex + 1];
        }
    }
}

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.CCLR,MessageType.Control,CommandType.Request,DeviceType.Manipulator)]
	public class CommandCCLR : BaseMessage
    {
        public string CMode;
        public CommandCCLR(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            CMode = _fields[_commandNameIndex + 1];
        }
    }
}

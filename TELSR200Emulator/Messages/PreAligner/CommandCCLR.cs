namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.CCLR,MessageType.Control,CommandType.Request,DeviceType.PreAligner)]
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

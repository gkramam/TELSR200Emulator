namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.SMSK,MessageType.Setting,CommandType.Request,DeviceType.PreAligner)]
	public class CommandSMSK : BaseMessage
    {
        public string Valid;
        public CommandSMSK(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            Valid = _fields[_commandNameIndex + 1];
        }
    }
}
namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.MACA,MessageType.Action,CommandType.Request,DeviceType.PreAligner)]
	public class CommandMACA : BaseMessage
    {
        public string Mode;
        public CommandMACA(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            Mode = _fields[_commandNameIndex + 1];
        }
    }
}

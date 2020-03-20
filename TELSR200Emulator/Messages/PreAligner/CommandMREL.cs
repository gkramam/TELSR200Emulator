namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.MREL,MessageType.Action,CommandType.Request,DeviceType.PreAligner)]
	public class CommandMREL : BaseMessage
    {
        public string Axis, Hand, Mode, Value;
        public CommandMREL(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            Axis = _fields[_commandNameIndex + 1];
            Hand = _fields[_commandNameIndex + 2];
            Mode = _fields[_commandNameIndex + 3];
            Value = _fields[_commandNameIndex + 4];
        }
    }
}

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.SPRM,MessageType.Setting,CommandType.Request,DeviceType.PreAligner)]
	public class CommandSPRM : BaseMessage
    {
        public string ParameterType, ParameterNumber, Value;
        public CommandSPRM(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            ParameterType = _fields[_commandNameIndex + 1];
            ParameterNumber = _fields[_commandNameIndex + 2];
            Value = _fields[_commandNameIndex + 3];
        }
    }
}
namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.SSTR,MessageType.Setting,CommandType.Request,DeviceType.Manipulator)]
	public class CommandSSTR : BaseMessage
    {
        public string MemorySpec, TransferStation, Item, Value;
        public CommandSSTR(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            MemorySpec = _fields[_commandNameIndex + 1];
            TransferStation = _fields[_commandNameIndex + 2];
            Item = _fields[_commandNameIndex + 3];
            Value = _fields[_commandNameIndex + 4];
        }
    }
}
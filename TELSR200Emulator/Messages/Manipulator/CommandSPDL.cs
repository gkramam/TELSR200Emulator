namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.SPDL,MessageType.Setting,CommandType.Request,DeviceType.Manipulator)]
	public class CommandSPDL : BaseMessage
    {
        public string MemorySpec, TransferStation, Posture, Hand;

        public CommandSPDL(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            MemorySpec = _fields[_commandNameIndex + 1];
            TransferStation = _fields[_commandNameIndex + 2];
            Posture = _fields[_commandNameIndex + 3];
            Hand = _fields[_commandNameIndex + 4];
        }
    }
}
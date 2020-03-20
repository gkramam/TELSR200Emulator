namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RSTP,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRSTP : BaseMessage
    {
        public string MemorySpec, TransferStation, Slot, Posture, Hand, PositionType;
        public ReferenceRSTP(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            MemorySpec = _fields[_commandNameIndex + 1];
            TransferStation = _fields[_commandNameIndex + 2];
            Slot = _fields[_commandNameIndex + 3];
            Posture = _fields[_commandNameIndex + 4];
            Hand = _fields[_commandNameIndex + 5];
            PositionType = _fields[_commandNameIndex + 6];
        }
    }
}

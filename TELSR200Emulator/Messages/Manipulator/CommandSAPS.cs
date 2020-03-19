namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandSAPS : BaseMessage
    {
        public string MemorySpec, RegistrationMode, TransferStation, Posture, Hand, OffsetX, OffsetY, OffsetZ;

        public CommandSAPS(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            MemorySpec = _fields[_commandNameIndex + 1];
            RegistrationMode = _fields[_commandNameIndex + 2];
            TransferStation = _fields[_commandNameIndex + 3];
            Posture = _fields[_commandNameIndex + 4];
            Hand = _fields[_commandNameIndex + 5];
            OffsetX = _fields[_commandNameIndex + 6];
            OffsetY = _fields[_commandNameIndex + 7];
            OffsetZ = _fields[_commandNameIndex + 8];
        }
    }
}
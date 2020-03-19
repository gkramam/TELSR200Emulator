namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandSPOS : BaseMessage
    {
        public string MemorySpec, RegistrationMode, TransferStation, Slot, Posture, Hand;
        public CommandSPOS(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            MemorySpec = _fields[_commandNameIndex + 1];
            RegistrationMode = _fields[_commandNameIndex + 2];
            TransferStation = _fields[_commandNameIndex + 3];
            Slot = _fields[_commandNameIndex + 4];
            Posture = _fields[_commandNameIndex + 5];
            Hand = _fields[_commandNameIndex + 6];
        }
    }
}
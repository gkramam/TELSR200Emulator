namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandSPLD : BaseMessage
    {
        public string TransferStation, Posture, Hand;

        public CommandSPLD(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            TransferStation = _fields[_commandNameIndex + 1];
            Posture = _fields[_commandNameIndex + 2];
            Hand = _fields[_commandNameIndex + 3];
        }
    }
}
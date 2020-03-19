namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMMCA:BaseMessage
    {
        public string TransferStation, Posture, Safe;
        public CommandMMCA(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            TransferStation = _fields[_commandNameIndex + 1];
            Posture = _fields[_commandNameIndex + 2];
            Safe = _fields[_commandNameIndex + 3];
        }
    }
}

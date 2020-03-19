namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandSTRM : BaseMessage
    {
        public string Mode1, Mode2, Mode3, Mode4, Mode5;
        public CommandSTRM(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            Mode1 = _fields[_commandNameIndex + 1];
            Mode2 = _fields[_commandNameIndex + 2];
            Mode3 = _fields[_commandNameIndex + 3];
            Mode4 = _fields[_commandNameIndex + 4];
            Mode5 = _fields[_commandNameIndex + 5];
        }
    }
}
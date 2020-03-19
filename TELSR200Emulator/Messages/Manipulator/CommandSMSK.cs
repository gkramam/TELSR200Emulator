namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandSMSK : BaseMessage
    {
        public string Valid;
        public CommandSMSK(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            Valid = _fields[_commandNameIndex + 1];
        }
    }
}
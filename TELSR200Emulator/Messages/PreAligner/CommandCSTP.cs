namespace TELSR200Emulator.Messages.PreAligner
{
    public class CommandCSTP : BaseMessage
    {
        public string StopMode;
        public CommandCSTP(string msg) : base(msg)
        {

        }

        public override void Parse()
        {
            base.Parse();
            StopMode = _fields[_commandNameIndex + 1];
        }
    }
}

namespace TELSR200Emulator.Messages.PreAligner
{
    public class CommandSSTD : BaseMessage
    {
        public string Axis;
        public CommandSSTD(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            Axis = _fields[_commandNameIndex + 1];
        }
    }
}
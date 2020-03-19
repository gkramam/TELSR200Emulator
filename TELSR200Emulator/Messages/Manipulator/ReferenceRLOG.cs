namespace TELSR200Emulator.Messages.Manipulator
{
    public class ReferenceRLOG : BaseMessage
    {
        public string LogNumber;
        public ReferenceRLOG(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            LogNumber = _fields[_commandNameIndex + 1];
        }
    }
}

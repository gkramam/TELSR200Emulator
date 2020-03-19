namespace TELSR200Emulator.Messages.Manipulator
{
    public class ReferenceRMCA : BaseMessage
    {
        public string TransferStation;
        public ReferenceRMCA(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            TransferStation = _fields[_commandNameIndex + 1];
        }
    }
}

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ReferenceRMAP : BaseMessage
    {
        public string TransferStation, Slot;
        public ReferenceRMAP(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            TransferStation = _fields[_commandNameIndex + 1];
            Slot = _fields[_commandNameIndex + 2];

        }
    }
}

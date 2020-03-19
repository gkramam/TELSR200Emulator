namespace TELSR200Emulator.Messages.Manipulator
{
    public class ReferenceRSTR : BaseMessage
    {
        public string MemorySpec, TransferStation, Item;
        public ReferenceRSTR(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            MemorySpec = _fields[_commandNameIndex + 1];
            TransferStation = _fields[_commandNameIndex + 2];
            Item = _fields[_commandNameIndex + 3];

        }
    }
}

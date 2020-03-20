namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RERR,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRERR : BaseMessage
    {
        public string MemorySpec, HNo;
        public ReferenceRERR(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            MemorySpec = _fields[_commandNameIndex + 1];
            HNo = _fields[_commandNameIndex + 2];

        }
    }
}

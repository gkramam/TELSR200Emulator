namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RLOG,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
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

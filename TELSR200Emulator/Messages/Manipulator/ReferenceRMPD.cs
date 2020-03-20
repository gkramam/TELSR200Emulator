namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RMPD,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRMPD : BaseMessage
    {
        public string TransferStation;
        public ReferenceRMPD(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            TransferStation = _fields[_commandNameIndex + 1];

        }
    }
}

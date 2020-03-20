namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RMCA,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
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

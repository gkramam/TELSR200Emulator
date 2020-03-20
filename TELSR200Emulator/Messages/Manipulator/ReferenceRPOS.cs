namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.RPOS,MessageType.Reference,CommandType.Request,DeviceType.Manipulator)]
	public class ReferenceRPOS : BaseMessage
    {
        public string PositionDataType;
        public ReferenceRPOS(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            PositionDataType = _fields[_commandNameIndex + 1];
        }
    }
}

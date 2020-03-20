namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.RACA,MessageType.Reference,CommandType.Request,DeviceType.PreAligner)]
	public class ReferenceRACA : BaseMessage
    {
        public string Level, SpeedType, Axis;
        public ReferenceRACA(string msg) : base(msg) { }
    }
}

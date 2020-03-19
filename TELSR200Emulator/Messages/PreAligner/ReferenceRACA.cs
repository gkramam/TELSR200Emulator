namespace TELSR200Emulator.Messages.PreAligner
{
    public class ReferenceRACA : BaseMessage
    {
        public string Level, SpeedType, Axis;
        public ReferenceRACA(string msg) : base(msg) { }
    }
}

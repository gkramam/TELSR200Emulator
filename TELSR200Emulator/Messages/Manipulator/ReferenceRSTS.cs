namespace TELSR200Emulator.Messages.Manipulator
{
    public class ReferenceRSTS : BaseMessage
    {
        public string TransferStation, Slot;
        public ReferenceRSTS(string msg) : base(msg) { }

    }
}

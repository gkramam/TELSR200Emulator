namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandACKN:BaseMessage
    {
        public CommandACKN(string message) : base(message) { }

        public override bool PerformMessageSpecificPreProcessing(Device device)
        {
            device.CommandState = DeviceState.Ready;
            device.IsReady = true;
            return false;
        }
    }
}

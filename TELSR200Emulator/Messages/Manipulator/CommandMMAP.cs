using System.Threading;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMMAP: BaseMessage
    {
        public string TransferStation, Slot, Posture, Safe;
        public CommandMMAP(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            TransferStation = _fields[_commandNameIndex + 1];
            Slot = _fields[_commandNameIndex + 2];
            Posture = _fields[_commandNameIndex + 3];
            Safe = _fields[_commandNameIndex + 4];
        }

        public override void PerformCommandSpecificProcessing(Device device)
        {
            Thread.Sleep(2000);
        }
    }
}

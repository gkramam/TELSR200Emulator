using System;

namespace TELSR200Emulator.Messages.PreAligner
{
    [Message(Messages.CommandName.CSRV,MessageType.Control,CommandType.Request,DeviceType.PreAligner)]
	public class CommandCSRV : BaseMessage
    {
        public string ServoCommand;
        public CommandCSRV(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            ServoCommand = _fields[_commandNameIndex + 1];
        }

        public override void PerformCommandSpecificProcessing(Device device)
        {
            device.IsServoOn = Convert.ToBoolean(int.Parse(ServoCommand));

            base.PerformCommandSpecificProcessing(device);
        }
    }
}

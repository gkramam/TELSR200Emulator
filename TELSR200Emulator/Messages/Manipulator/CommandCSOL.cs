using System;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.CSOL,MessageType.Control,CommandType.Request,DeviceType.Manipulator)]
	public class CommandCSOL : BaseMessage
    {
        public string SolenoidControlSpec, SolenoidCommand;
        public bool ShouldWait;
        public CommandCSOL(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            SolenoidControlSpec = _fields[_commandNameIndex + 1];
            SolenoidCommand = _fields[_commandNameIndex + 2];
            ShouldWait = Convert.ToBoolean(Int32.Parse(_fields[_commandNameIndex + 3]));
        }
    }
}

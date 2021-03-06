﻿namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.MPNT,MessageType.Action,CommandType.Request,DeviceType.Manipulator)]
	public class CommandMPNT : BaseMessage
    {
        public string TransferPoint;
        public CommandMPNT(string msg) : base(msg)
        {

        }

        public override void Parse()
        {
            base.Parse();

            TransferPoint = _fields[_commandNameIndex + 1];
        }

        public override void PerformPostEOESend(CommandContext ctxt, Device device)
        {
            Emulation.preAligner.RaiseAlignmentStatusResultEvent();
        }
    }
}

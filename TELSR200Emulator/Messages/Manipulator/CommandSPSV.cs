using System;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandSPSV : BaseMessage
    {
        public string TransferStation, Posture, Hand;

        public CommandSPSV(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            TransferStation = _fields[_commandNameIndex + 1];
            Posture = _fields[_commandNameIndex + 2];
            Hand = _fields[_commandNameIndex + 3];
        }
    }
}
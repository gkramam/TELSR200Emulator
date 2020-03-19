using System;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class CommandSSPD : BaseMessage
    {
        public string Level, SpeedType, Axis, Speed;
        public CommandSSPD(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            Level = _fields[_commandNameIndex + 1];
            SpeedType = _fields[_commandNameIndex + 2];
            Axis = _fields[_commandNameIndex + 3];
            Speed = _fields[_commandNameIndex + 4];
        }
    }
}
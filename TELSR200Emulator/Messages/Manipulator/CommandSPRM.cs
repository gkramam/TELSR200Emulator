using System;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandSPRM : BaseMessage
    {
        public string ParameterType,ParameterNumber,Value;
        public CommandSPRM(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            ParameterType = _fields[_commandNameIndex + 1];
            ParameterNumber = _fields[_commandNameIndex + 2];
            Value = _fields[_commandNameIndex + 3];
        }
    }
}
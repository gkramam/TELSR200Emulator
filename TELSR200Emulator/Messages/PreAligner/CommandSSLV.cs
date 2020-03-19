using System;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class CommandSSLV : BaseMessage
    {
        public string Level;
        public CommandSSLV(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            Level = _fields[_commandNameIndex + 1];
        }
    }
}
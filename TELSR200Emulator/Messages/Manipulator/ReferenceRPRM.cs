namespace TELSR200Emulator.Messages.Manipulator
{
    public class ReferenceRPRM : BaseMessage
    {
        public string ParameterType, ParameterNumber;
        public ReferenceRPRM(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            ParameterType = _fields[_commandNameIndex + 1];
            ParameterNumber = _fields[_commandNameIndex + 2];

        }
    }
}

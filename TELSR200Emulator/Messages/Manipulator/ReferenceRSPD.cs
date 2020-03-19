namespace TELSR200Emulator.Messages.Manipulator
{
    public class ReferenceRSPD : BaseMessage
    {
        public string Level, SpeedType, Axis;
        public ReferenceRSPD(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            Level = _fields[_commandNameIndex + 1];
            SpeedType = _fields[_commandNameIndex + 2];
            Axis = _fields[_commandNameIndex + 3];

        }
    }
}

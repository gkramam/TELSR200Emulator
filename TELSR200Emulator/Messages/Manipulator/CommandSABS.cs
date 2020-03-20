namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.SABS,MessageType.Setting,CommandType.Request,DeviceType.Manipulator)]
	public class CommandSABS : BaseMessage
    {
        public string MemorySpec, RegistrationMode, TransferStation, Posture, Hand, RotationAxis, ExtensionAxis, WristAxis1, WristAxis2, ElevationAxis;

        public CommandSABS(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();
            MemorySpec = _fields[_commandNameIndex + 1];
            RegistrationMode = _fields[_commandNameIndex + 2];
            TransferStation = _fields[_commandNameIndex + 3];
            Posture = _fields[_commandNameIndex + 4];
            Hand = _fields[_commandNameIndex + 5];
            RotationAxis = _fields[_commandNameIndex + 6];
            ExtensionAxis = _fields[_commandNameIndex + 7];
            WristAxis1 = _fields[_commandNameIndex + 8];
            WristAxis2 = _fields[_commandNameIndex + 9];
            ElevationAxis = _fields[_commandNameIndex + 10];
        }
    }
}
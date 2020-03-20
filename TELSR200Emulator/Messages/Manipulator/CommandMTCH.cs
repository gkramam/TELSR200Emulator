using System.Linq;

namespace TELSR200Emulator.Messages.Manipulator
{
    [Message(Messages.CommandName.MTCH,MessageType.Action,CommandType.Request,DeviceType.Manipulator)]
	public class CommandMTCH : BaseMessage
    {
        public string TransferStation;
        public string Slot;
        public string Posture;
        public string Hand;
        public string PMode;
        public bool OffsetSpecified;
        public string OffsetX, OffsetY, OffsetZ;

        public CommandMTCH(string msg) : base(msg)
        {

        }

        override public void Parse()
        {
            base.Parse();

            TransferStation = _fields[_commandNameIndex + 1];
            Slot = _fields[_commandNameIndex + 2];
            Posture = _fields[_commandNameIndex + 3];
            Hand = _fields[_commandNameIndex + 4];
            PMode = _fields[_commandNameIndex + 5];

            if (_fields.Count() > _commandNameIndex + 1 + 5)
            {
                OffsetSpecified = true;
                OffsetX = _fields[_commandNameIndex + 6];
                OffsetY = _fields[_commandNameIndex + 7];
                OffsetZ = _fields[_commandNameIndex + 8];
            }
        }
    }
}

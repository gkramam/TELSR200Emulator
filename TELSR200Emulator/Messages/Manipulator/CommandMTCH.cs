using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMTCH: BaseMessage
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

            TransferStation = _fields[2];
            Slot = _fields[3];
            Posture = _fields[4];
            Hand = _fields[5];
            PMode = _fields[6];

            if (_fields.Count() > 7)
            {
                OffsetSpecified = true;
                OffsetX = _fields[7];
                OffsetY = _fields[8];
                OffsetZ = _fields[9];
            }
        }
    }
}

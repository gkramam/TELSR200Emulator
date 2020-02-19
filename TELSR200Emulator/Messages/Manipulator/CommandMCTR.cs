using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMCTR: BaseMessage
    {
        public string MotionMode;
        public string TransferStation;
        public string Slot;
        public string Posture;
        public string Hand;
        public string TransferPoint;
        public bool OffsetSpecified;
        public bool AngleSpecified;
        public string OffsetX, OffsetY, OffsetZ;
        public string Angle;
        public CommandMCTR(string msg) : base(msg) { }

        override public void Parse()
        {
            base.Parse();

            MotionMode = _fields[2];
            TransferStation = _fields[3];
            Slot = _fields[4];
            Posture = _fields[5];
            Hand = _fields[6];
            TransferPoint = _fields[7];

            if (_fields.Count() > 8)
            {
                if (_fields.Count() == 12)
                {
                    OffsetSpecified = AngleSpecified = true;
                    OffsetX = _fields[8];
                    OffsetY = _fields[9];
                    OffsetZ = _fields[10];
                    Angle = _fields[11];
                }
                else if (_fields.Count() == 11)
                {
                    OffsetSpecified = true;
                    AngleSpecified = false;
                    OffsetX = _fields[8];
                    OffsetY = _fields[9];
                    OffsetZ = _fields[10];
                }
                else if (_fields.Count() == 9)
                {
                    OffsetSpecified = false;
                    AngleSpecified = true;
                    Angle = _fields[8];
                }
            }
        }
    }
}

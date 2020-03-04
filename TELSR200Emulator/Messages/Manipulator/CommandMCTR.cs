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

            MotionMode = _fields[_commandNameIndex +1];
            TransferStation = _fields[_commandNameIndex +2];
            Slot = _fields[_commandNameIndex +3];
            Posture = _fields[_commandNameIndex +4];
            Hand = _fields[_commandNameIndex +5];
            TransferPoint = _fields[_commandNameIndex + 6];

            if (_fields.Count() > _commandNameIndex+1+6)
            {
                if (_fields.Count() == _commandNameIndex+1+6+4)
                {
                    OffsetSpecified = AngleSpecified = true;
                    OffsetX = _fields[_commandNameIndex + 7];
                    OffsetY = _fields[_commandNameIndex + 8];
                    OffsetZ = _fields[_commandNameIndex +9];
                    Angle = _fields[_commandNameIndex + 10];
                }
                else if (_fields.Count() == _commandNameIndex+1+6+3)
                {
                    OffsetSpecified = true;
                    AngleSpecified = false;
                    OffsetX = _fields[_commandNameIndex + 7];
                    OffsetY = _fields[_commandNameIndex +8];
                    OffsetZ = _fields[_commandNameIndex +9];
                }
                else if (_fields.Count() == _commandNameIndex+1+6+1)
                {
                    OffsetSpecified = false;
                    AngleSpecified = true;
                    Angle = _fields[_commandNameIndex +7];
                }
            }
        }

        public override void PerformPostEOESend(CommandContext ctxt, Device device)
        {
            Emulation.preAligner.RaiseAlignmentStatusResultEvent();
        }
    }
}

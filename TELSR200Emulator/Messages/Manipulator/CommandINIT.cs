﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandINIT : BaseMessage
    {
        public bool ErrorClear;
        public bool ServoOn;
        public string HomeAxis;

        public CommandINIT(string message) : base(message)
        {

        }

        override public void Parse()
        {
            base.Parse();

            if (_fields[_commandNameIndex+1] == "1")
                ErrorClear = true;

            if (_fields[_commandNameIndex +2] == "1")
                ServoOn = true;

            HomeAxis = _fields[_commandNameIndex +3];
        }

        public override bool PerformMessageSpecificPreProcessing(Device device)
        {
            device.Reset();

            return base.PerformMessageSpecificPreProcessing(device);
        }

        public override void PerformCommandSpecificProcessing(Device device)
        {
            if (ErrorClear)
                device.IsError = false;

            if (ServoOn)
                device.IsServoOn = true;

            device.GoHome(HomeAxis.ToCharArray()[0]);

            base.PerformCommandSpecificProcessing(device);
        }
    }
}

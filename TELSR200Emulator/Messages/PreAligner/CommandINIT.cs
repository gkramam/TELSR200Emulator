﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.PreAligner
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

            if (_fields[_commandNameIndex + 1] == "1")
                ErrorClear = true;
            if (_fields[_commandNameIndex + 2] == "1")
                ServoOn = true;
            HomeAxis = _fields[_commandNameIndex + 3];
        }
    }
}
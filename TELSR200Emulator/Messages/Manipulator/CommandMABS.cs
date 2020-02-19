﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class CommandMABS:BaseMessage
    {
        public string Axis,Hand, Mode, Value;
        public CommandMABS(string msg) : base(msg) { }

        public override void Parse()
        {
            base.Parse();

            Axis = _fields[_commandNameIndex + 1];
            Hand = _fields[_commandNameIndex + 2];
            Mode = _fields[_commandNameIndex + 3];
            Value = _fields[_commandNameIndex + 4];
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class EndOfExecMREL:EndOfExecGeneric
    {
        public EndOfExecMREL(BaseMessage req) : base(req) { }
    }
}

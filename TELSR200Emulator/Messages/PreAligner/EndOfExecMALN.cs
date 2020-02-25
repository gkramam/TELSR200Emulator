﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class EndOfExecMALN : BaseEndOfExec
    {
        public EndOfExecMALN(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            var preAligner = (Devices.PreAligner)device;

            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');
            _responseBuilder.Append(preAligner.BuildEOEMALN(_request));

            return base.Generate(device);
        }
    }
}

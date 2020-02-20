﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseMMCA:BaseResponse
    {
        public ResponseMMCA(CommandMMCA req) : base(req) { }

        public override string Generate()
        {
            _responseBuilder = new StringBuilder();

            CommandMMCA req = (CommandMMCA)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Safe);

            return base.Generate();
        }
    }
}
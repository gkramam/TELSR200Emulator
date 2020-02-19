﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseMTRS:BaseResponse
    {
        public ResponseMTRS(CommandMTRS request) : base(request) 
        {
            _sts1 = ResponseSts1.UnitReady;
            _sts2 = ResponseSts2.None;
            _request = request;
        }

        public override string Generate()
        {
            _responseBuilder = new StringBuilder();

            CommandMTRS req = (CommandMTRS)_request;

            _responseBuilder.Append(req.MotionMode);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Slot);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Posture);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Hand);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.TransferPoint);

            if (req.OffsetSpecified)
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetX);
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetY);
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.OffsetZ);
            }

            if (req.AngleSpecified)
            {
                _responseBuilder.Append(',');
                _responseBuilder.Append(req.Angle);
            }

            return base.Generate();
        }
    }
}

﻿using System;

namespace TELSR200Emulator.Messages
{
    public class BaseEndOfExec : BaseResponse
    {
        protected TimeSpan _executionTime;
        public BaseEndOfExec(BaseMessage req) : base(req)
        {
            _executionTime = TimeSpan.FromMilliseconds(33);//33 mills
        }

        public override string Generate(Device device)
        {
            return base.Generate(device);

        }
    }
}
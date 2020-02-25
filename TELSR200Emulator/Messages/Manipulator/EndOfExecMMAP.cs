using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Devices;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecMMAP:BaseEndOfExec
    {
        public EndOfExecMMAP(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            var robot = (Devices.Manipulator)device;

            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');
            _responseBuilder.Append(robot.BuildEOEMMAP(_request));

            return base.Generate(device);
        }
    }
}

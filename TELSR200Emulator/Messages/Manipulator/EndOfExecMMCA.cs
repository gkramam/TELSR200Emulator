using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecMMCA: BaseEndOfExec
    {
        public EndOfExecMMCA(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            var robot = (Devices.Manipulator)device;

            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');
            _responseBuilder.Append(robot.BuildEOEMMCA(_request));

            return base.Generate(device);
        }
    }
}

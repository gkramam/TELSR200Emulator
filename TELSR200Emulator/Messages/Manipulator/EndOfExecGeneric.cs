using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecGeneric : BaseEndOfExec
    {

        public EndOfExecGeneric(BaseMessage request) : base(request)
        {
        }

        public override string Generate(Device device)
        {
            var robot = (Devices.Manipulator)device;

            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');
            _responseBuilder.Append(robot.BuildEOEGeneric(_request));
            
            return base.Generate(device);
        }
    }
}

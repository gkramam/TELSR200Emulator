using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseCSRV:BaseResponse
    {
        public ResponseCSRV(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            Devices.Manipulator robot = (Devices.Manipulator)device;
            CommandCSRV req = (CommandCSRV)_request;

            _responseBuilder.Append(req.ServoCommand);

            return base.Generate(device);
        }
    }
}

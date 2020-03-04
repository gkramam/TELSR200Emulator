using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class ResponseRMAP:BaseResponse
    {
        public ResponseRMAP(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            Devices.Manipulator robot = (Devices.Manipulator)device;
            ReferenceRMAP req = (ReferenceRMAP)_request;

            _responseBuilder.Append(req.TransferStation);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Slot);
            _responseBuilder.Append(',');
            _responseBuilder.Append(robot.GetMappingResult(req.TransferStation, req.Slot));

            return base.Generate(device);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Configuration;


namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecMMCA: BaseEndOfExec
    {
        public EndOfExecMMCA(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            var robot = (Devices.Manipulator)device;
            var req = (CommandMMCA)_request;
            var robotConfig = AppConfiguration.environment.Manipulator;

            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');

            var station = robotConfig.Stations.Where(s => s.ID.Equals(req.TransferStation)).FirstOrDefault();

            if(station == null)
                throw new ApplicationException($"Couldn't find station {req.TransferStation}. This may be because the station is not added in the environment file. Please check");

            robot.MoveToPosition(new Devices.RobotCoordinates(station.LowestRegisteredPosition));
            _responseBuilder.Append(robot.CurrentPositionPosture.ToString());
            _responseBuilder.Append(',');
            _responseBuilder.Append(station.GetCalibrationResult());

            return base.Generate(device);
        }
    }
}

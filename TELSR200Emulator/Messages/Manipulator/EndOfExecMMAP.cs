using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Configuration;
using TELSR200Emulator.Devices;

namespace TELSR200Emulator.Messages.Manipulator
{
    public class EndOfExecMMAP:BaseEndOfExec
    {
        public EndOfExecMMAP(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            var robot = (Devices.Manipulator)device;
            var req = (CommandMMAP)_request;
            var robotConfig = AppConfiguration.environment.Manipulator;

            _responseBuilder.Append(_executionTime.ToString("ffffff"));
            _responseBuilder.Append(',');

            Configuration.Station station = null;

            switch (req.TransferStation.FirstOrDefault())
            {
                case 'C':
                    station = robotConfig.Stations.Where(s => s.Type == Configuration.StationType.Casette && s.ID.Equals(req.TransferStation)).FirstOrDefault();
                    break;
                case 'S':
                    station = robotConfig.Stations.Where(s => s.Type == Configuration.StationType.Transfer && s.ID.Equals(req.TransferStation)).FirstOrDefault();
                    break;
                case 'P':
                    station = robotConfig.Stations.Where(s => s.Type == Configuration.StationType.PreAligner && s.ID.Equals(req.TransferStation)).FirstOrDefault();
                    break;
            }

            if (station == null)
                throw new ApplicationException($"Couldn't find station {req.TransferStation}. This may be because the station is not added in the environment file. Please check");
            
            robot.MoveToPosition(new Devices.RobotCoordinates(station.LowestRegisteredPosition));

            _responseBuilder.Append(robot.CurrentPositionPosture.ToString());
            _responseBuilder.Append(',');

            _responseBuilder.Append(station.GetMappingStatus(req.Slot));
            
            return base.Generate(device);
        }

    }

}

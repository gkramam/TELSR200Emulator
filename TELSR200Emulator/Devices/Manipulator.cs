using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator.Devices
{
    public class Manipulator : Device
    {
        private static readonly object _lock = new object();

        public override int UnitNumber
        {
            get
            {
                return 1;
            }
        }
        public bool IsWaferPresentOnBlade1 { get; set; }

        public bool IsWaferPresentOnBlade2 { get; set; }

        public RobotCoordinates HomePositionPosture { get; set; }

        public RobotCoordinates CurrentPositionPosture { get; set; }

        Configuration.Manipulator Configuration;

        Dictionary<string, string> _mappingResults;
        public Dictionary<string, string> MappingResults
        {
            get
            {
                lock (_lock)
                {
                    return _mappingResults;
                }
            }
            set
            {
                lock (_lock)
                {
                    _mappingResults = value;
                }
            }
        }

        public string GetMappingResult(string stationID, string slotID)
        {
            var robotConfig = AppConfiguration.environment.Manipulator;
            Configuration.Station station = robotConfig.Stations.Where(s => s.ID.Equals(stationID)).FirstOrDefault();
            if (station == null)
                throw new ApplicationException($"Couldn't find station {stationID}. This may be because the station is not added in the environment file. Please check");
            return station.GetMappingStatus(slotID);
        }
        public Manipulator()
        {
            Configuration = AppConfiguration.environment.Manipulator;
            HomePositionPosture = new RobotCoordinates(Configuration.HomePosition);
            CurrentPositionPosture = HomePositionPosture;
        }

        public void ProcessACKN(CommandContext ctxt)
        {
            IsReady = true;
            Console.WriteLine("ACKN Received");
        }

        public override void Reset()
        {
            IsWaferPresentOnBlade1 = false;
            IsWaferPresentOnBlade2 = false;
            base.Reset();
        }

        public override void GoHome(char axesCode)
        {
            if (axesCode == 'G')
            {
                CurrentPositionPosture = HomePositionPosture;
            }
            else if (axesCode == 'A')
            {
                CurrentPositionPosture.WristAxis1 = HomePositionPosture.WristAxis1;
                CurrentPositionPosture.WristAxis2 = HomePositionPosture.WristAxis2;
            }
        }

        public override ResponseStatus2 GetResponseStatus2()
        {
            ResponseStatus2 ret = ResponseStatus2.None;
            if (IsBatteryVoltageDropped)
                ret = ret | ResponseStatus2.BattVoltDropped;
            if (IsWaferPresentOnBlade1)
                ret = ret | ResponseStatus2.Blade1_Vac_Grip_HasWafer;
            if (IsWaferPresentOnBlade2)
                ret = ret | ResponseStatus2.Blade2_LineSensor_Haswafer;
            return ret;
        }

        public void MoveToPosition(RobotCoordinates newPosition)
        {
            CurrentPositionPosture = newPosition;
        }

    }

    public class RobotCoordinates
    {
        public double RotationAxis { get; set; }
        public double ExtensionAxis { get; set; }
        public double WristAxis1 { get; set; }
        public double WristAxis2 { get; set; }
        public double ElevationAxis { get; set; }

        public RobotCoordinates() { }

        public RobotCoordinates(Configuration.ManipulatorPosition registeredPos)
        {
            (RotationAxis, ExtensionAxis, WristAxis1, WristAxis2, ElevationAxis) =
                (registeredPos.RotationAxis, registeredPos.ExtensionAxis, registeredPos.WristAxis1, registeredPos.WristAxis2, registeredPos.ElevationAxis);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(((int)(RotationAxis / 0.001)).ToString("D8"));
            builder.Append(',');
            builder.Append(((int)(ExtensionAxis / 0.001)).ToString("D8"));
            builder.Append(',');
            builder.Append(((int)(WristAxis1 / 0.001)).ToString("D8"));
            builder.Append(',');
            builder.Append(((int)(WristAxis2 / 0.001)).ToString("D8"));
            builder.Append(',');
            builder.Append(((int)(ElevationAxis / 0.001)).ToString("D8"));
            return builder.ToString();
        }
    }
}

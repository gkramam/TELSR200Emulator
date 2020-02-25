using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;
using TELSR200Emulator.Messages.Manipulator;

namespace TELSR200Emulator.Devices
{
    public class Manipulator: Device
    {
        public bool IsWaferPresentOnBlade1 { get; set; }

        public bool IsWaferPresentOnBlade2 { get; set; }

        public RobotCoordinates HomePositionPosture { get; set; }

        public RobotCoordinates CurrentPositionPosture { get; set; }
        public Manipulator()
        {
            HomePositionPosture = new RobotCoordinates() { RotationAxis = 0, ExtensionAxis = 0, WristAxis1 = 90, WristAxis2 = 90, ElevationAxis = 0 };
            CurrentPositionPosture = HomePositionPosture;
        }

        public string BuildEOEGeneric(BaseMessage request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("00000100");//pos1
            builder.Append(',');
            builder.Append("00000100");//pos2
            builder.Append(',');
            builder.Append("00000100");//pos3
            builder.Append(',');
            builder.Append("00000100");//pos4
            builder.Append(',');
            builder.Append("00000100");//pos5
            return builder.ToString();
        }

        public void ProcessMMAP()
        {
            IsReady = false;
            //Thread.Sleep(1000);
        }
        public string BuildEOEMMAP(BaseMessage request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("00000100");//pos1
            builder.Append(',');
            builder.Append("00000100");//pos2
            builder.Append(',');
            builder.Append("00000100");//pos3
            builder.Append(',');
            builder.Append("00000100");//pos4
            builder.Append(',');
            builder.Append("00000100");//pos5
            builder.Append(',');
            CommandMMAP req = (CommandMMAP)request;
            builder.Append(req.Slot);
            builder.Append(":OK");
            return builder.ToString();
        }

        public void ProcessMMCA()
        {
            IsReady = false;
            //Thread.Sleep(1000);
        }

        public string BuildEOEMMCA(BaseMessage request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("00000100");//pos1
            builder.Append(',');
            builder.Append("00000100");//pos2
            builder.Append(',');
            builder.Append("00000100");//pos3
            builder.Append(',');
            builder.Append("00000100");//pos4
            builder.Append(',');
            builder.Append("00000100");//pos5
            builder.Append(',');
            builder.Append("00000100");//Lowest-slot position
            builder.Append(',');
            builder.Append("00000100");//Lowest-slot position
            builder.Append(',');
            builder.Append("00000100");//Lowest-slot position
            builder.Append(',');
            builder.Append("00000100");//Lowest-slot position
            builder.Append(',');
            builder.Append("00000100");//Lowest-slot position
            builder.Append(',');
            builder.Append("00000100");//Lowest-slot position
            return builder.ToString();
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
    }

    public class RobotCoordinates
    {
        public double RotationAxis { get; set; }
        public double ExtensionAxis { get; set; }
        public double WristAxis1 { get; set; }
        public double WristAxis2 { get; set; }
        public double ElevationAxis { get; set; }
    }
}

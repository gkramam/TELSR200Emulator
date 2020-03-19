using System;
using System.Text;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator.Devices
{
    public class PreAligner : Device
    {
        public override int UnitNumber
        {
            get
            {
                return 2;
            }
        }
        public bool IsWaferPresentOnLineSensor { get; set; }

        public bool IsWaferPresentOnVacuumOrGripSensor { get; set; }

        public PreAlignerCoordinates HomePositionPosture { get; set; }
        public PreAlignerCoordinates CurrentPositionPosture { get; set; }

        public PreAligner()
        {
            HomePositionPosture = new PreAlignerCoordinates() { RotationAxis = 0 };
            CurrentPositionPosture = HomePositionPosture;
        }

        public string BuildEOEGeneric(BaseMessage request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("00000000");//pos1
            return builder.ToString();
        }

        public void ProcessMALN()
        {
            IsReady = false;
            //Thread.Sleep(1000);
        }
        public string BuildEOEMALN(BaseMessage request)
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

        public void RaiseAlignmentStatusResultEvent()
        {
            AlignmentResultEvent evt = new AlignmentResultEvent();
            var evtmsg = evt.Generate(UnitNumber, "0000", BuildEOEMALN(null));
            Emulation.preAlignerTcpWorker.ActiveConnection.QResponse(evtmsg);
        }

        public void ProcessMACA()
        {
            IsReady = false;
            //Thread.Sleep(50);
        }
        public string BuildEOEMACA(BaseMessage request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("00000100");//pos1
            builder.Append(',');
            builder.Append("00000100");//pos2
            builder.Append(',');
            builder.Append("00000100");//pos3
            return builder.ToString();
        }

        public override void Reset()
        {
            IsBatteryVoltageDropped = false;
            IsWaferPresentOnLineSensor = false;
            IsWaferPresentOnVacuumOrGripSensor = false;
            base.Reset();
        }

        public override void GoHome(char axesCode)
        {
            if (axesCode == 'G')
                CurrentPositionPosture = HomePositionPosture;
        }
        public void ProcessACKN(CommandContext ctxt)
        {
            IsReady = true;
            Console.WriteLine("ACKN Received");
        }

        public override ResponseStatus2 GetResponseStatus2()
        {
            ResponseStatus2 ret = ResponseStatus2.None;
            if (IsBatteryVoltageDropped)
                ret = ret | ResponseStatus2.BattVoltDropped;
            if (IsWaferPresentOnVacuumOrGripSensor)
                ret = ret | ResponseStatus2.Blade1_Vac_Grip_HasWafer;
            if (IsWaferPresentOnLineSensor)
                ret = ret | ResponseStatus2.Blade2_LineSensor_Haswafer;
            return ret;
        }

    }

    public class PreAlignerCoordinates
    {
        public double RotationAxis { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator.Devices
{
    public class PreAligner
    {
        private bool _ready = false;
        public PreAligner() { }

        public void Process<C, R, E>(CommandContext ctxt, Action processCB, Func<BaseMessage, string> buildEOECB) where R : BaseResponse where C : BaseMessage where E : BaseEndOfExec
        {
            C req = (C)Activator.CreateInstance(typeof(C), ctxt.CommandMessage);
            req.Parse();

            //Update state and introduce time delays here.

            R reply = (R)Activator.CreateInstance(typeof(R), req);

            var response = reply.Generate();

            ctxt.ResponseQCallback(response);

            _ready = false;

            //Thread.Sleep(3000);

            E endOfExec = (E)Activator.CreateInstance(typeof(E), req);
            var endProcessing = endOfExec.Generate(processCB, buildEOECB);

            ctxt.ResponseQCallback(endProcessing);
        }

        public void ProcessGeneric()
        {
            _ready = false;
            //Thread.Sleep(1000);
        }
        public string BuildEOEGeneric(BaseMessage request)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("00000000");//pos1
            return builder.ToString();
        }

        public void ProcessMALN()
        {
            _ready = false;
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

        public void ProcessMACA()
        {
            _ready = false;
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
        public void ProcessACKN(CommandContext ctxt)
        {
            _ready = true;
            Console.WriteLine("ACKN Received");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TELSR200Emulator.Devices
{
    public class Manipulator
    {
        bool _ready = true;
        //private readonly int _port;
        public Manipulator()
        {
        }

        public void Process<C, R, E>(CommandContext ctxt) where R : BaseResponse where C : BaseMessage where E : BaseEndOfExec
        {
            C req = (C)Activator.CreateInstance(typeof(C), ctxt.CommandMessage);
            req.Parse();

            //Update state and introduce time delays here.

            R reply = (R)Activator.CreateInstance(typeof(R), req);

            var response = reply.Generate();

            ctxt.ResponseQCallback(response);

            _ready = false;

            Thread.Sleep(3000);

            E endOfExec = (E)Activator.CreateInstance(typeof(E), req);
            var endProcessing = endOfExec.Generate();

            ctxt.ResponseQCallback(endProcessing);
        }

        public void ProcessACKN(CommandContext ctxt)
        {
            _ready = true;
        }
    }
}

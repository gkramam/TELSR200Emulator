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
        //private readonly int _port;
        public Manipulator()
        {
        }

        public void Process<C,R>(CommandContext ctxt) where R : BaseResponse where C:BaseMessage
        {
            C req = (C)Activator.CreateInstance(typeof(C), ctxt.CommandMessage);
            req.Parse();

            //Update state and introduce time delays here.

            Thread.Sleep(3000);

            R reply = (R)Activator.CreateInstance(typeof(R), req);

            var response = reply.Generate();

            ctxt.ResponseQCallback(response);
        }
    }
}

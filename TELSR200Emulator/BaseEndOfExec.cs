using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public class BaseEndOfExec: BaseResponse
    {
        public BaseEndOfExec(BaseMessage req) : base(req) { }
    }
}
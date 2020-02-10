using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    abstract public class BaseResponse
    {
        protected BaseMessage _request;
        public BaseResponse(BaseMessage request)
        {
            _request = request;
        }

        public abstract string Generate();
        
    }
}

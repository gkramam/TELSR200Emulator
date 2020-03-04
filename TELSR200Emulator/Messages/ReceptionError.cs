using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages
{
    public class ReceptionError
    {
        public static string Generate(string errorCode)
        {
            StringBuilder ret = new StringBuilder();

            ret.Append($",{errorCode}");

            if (AppConfiguration.checkSumCheck)
            {
                var chksum = CheckSum.Compute(ret.ToString());
                ret.Append(',');
                ret.Append(chksum);
            }

            ret.Append('\r');
            ret.Insert(0, '?');
            return ret.ToString();
        }
    }
}

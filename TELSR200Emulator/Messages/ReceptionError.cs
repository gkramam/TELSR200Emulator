using System.Text;

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

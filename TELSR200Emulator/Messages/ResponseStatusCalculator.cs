using System;
using System.Text;

namespace TELSR200Emulator.Messages
{
    public class ResponseStatusCalculator
    {
        public static string Calculate(byte status)
        {
            //var stsStr = status.ToString("X");
            var hexsts1 = BitConverter.ToString(new byte[] { status });//Getting the Hex Value
            var asciiSts1 = Encoding.ASCII.GetBytes(hexsts1.ToCharArray(), 1, 1);
            return Encoding.ASCII.GetString(asciiSts1);
        }
    }
}

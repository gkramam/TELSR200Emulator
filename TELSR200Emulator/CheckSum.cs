using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public static class CheckSum
    {
        public static string Compute(string unicodeMsg)
        {
            try
            {
                //unicodeMsg = ",1,INIT,1,1,G,";
                byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(unicodeMsg);
                //var hexString = BitConverter.ToString(asciiBytes);
                int count = 0;
                for (int i = 0; i < asciiBytes.Length; i++)
                {
                    count += asciiBytes[i];
                }

                var hexsumBytes = BitConverter.GetBytes(count);
                var hexsumString = BitConverter.ToString(hexsumBytes, 0, 1);
                return hexsumString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                throw;
            }
        }

        public static bool IsValid(string message, string rcvdCheckSum)
        {
            var computed = Compute(message.ToString());

            if (computed.Equals(rcvdCheckSum))
                return true;
            else
            {
                Console.WriteLine($"computed checksum: {computed} \t rcvd: {rcvdCheckSum}");
                return false;
            }
        }
    }
}

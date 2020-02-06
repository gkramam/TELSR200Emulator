using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator
{
    public class Logger
    {
        private static Logger _instance;

        public static Logger Instance
        {
            get { return _instance; }
        }

        private Logger() { }

        static Logger()
        {
            _instance = new Logger();
        }

        public void Log(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            Console.WriteLine($"{file} - {member} - {line}");
            Console.WriteLine(message);
        }

        public void Log(Exception e, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            Console.WriteLine($"{file} - {member} - {line}");
            Console.WriteLine(e.Message);
        }
    }
}

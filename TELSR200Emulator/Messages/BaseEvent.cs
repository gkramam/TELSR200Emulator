using System;
using System.Text;

namespace TELSR200Emulator.Messages
{
    public class BaseEvent
    {
        protected StringBuilder _builder;
        //public BaseEvent()
        //{
        //    _builder = new StringBuilder();
        //}
        public virtual string Generate(int device, string errorCode, string result = null)
        {
            var date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss,");
            _builder.Insert(4, date);
            _builder.Insert(0,$",{device},EVNT,");

            if (AppConfiguration.checkSumCheck)
            {
                var chksum = CheckSum.Compute(_builder.ToString());
                _builder.Append(',');
                _builder.Append(chksum);
            }

            _builder.Append('\r');
            _builder.Insert(0, '>');
            return _builder.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TELSR200Emulator.Messages
{
    public class BaseMessage
    {
        private string _message;

        protected string[] _fields;
        protected int _commandNameIndex;

        private int? _seqNum;
        public int? SeqNum
        {
            get { return _seqNum; }
        }

        private int _unitNumber;
        public int UnitNumber
        {
            get
            {
                return _unitNumber;
            }
        }

        private string _commandName;
        public string CommandName
        {
            get
            {
                return _commandName;
            }
        }
        public BaseMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new Exception("message cannot be null");
            _message = message;
            _commandNameIndex = AppConfiguration.useSequenceNumber ? 2 : 1;
        }

        public virtual void Parse()
        {
            var strippeedCmd = _message.Substring(2, _message.Length - 6);
            _fields = strippeedCmd.Split(',');
            
            _unitNumber = Convert.ToInt32(_fields[0]);
            
            if(AppConfiguration.useSequenceNumber)
                _seqNum = Convert.ToInt32(_fields[1]);
            
            _commandName = _fields[_commandNameIndex];
        }
    }

    [Flags]
    public enum ResponseSts1 : short
    {
        None = 0x0,
        UnitReady = 0x2,//1 ready, 0 busy
        ServoOff = 0x4,
        ErrorOccured = 0x8
    }

    [Flags]
    public enum ResponseSts2 : short
    {
        None = 0x0,
        BattVoltDropped = 0x1,
        Blade1_Vac_Grip_HasWafer = 0x2,//1 ready, 0 busy
        Blade2_LineSensor_Haswafer = 0x4,
    }
}

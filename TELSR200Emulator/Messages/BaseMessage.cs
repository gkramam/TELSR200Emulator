using System;
using System.Linq;
using TELSR200Emulator.Messages.PreAligner;

namespace TELSR200Emulator.Messages
{
    public abstract class BaseMessage
    {
        private string _message;

        protected string[] _fields;
        protected int _commandNameIndex;

        private int? _seqNum;
        public int? SeqNum { get => _seqNum; }

        private int _unitNumber;
        public int UnitNumber { get => _unitNumber; }

        private string _commandName;
        public string CommandName { get => _commandName; }

        public MessageType Type { get; set; }

        public Type ResponseType { get; set; }

        public Type EndOfExecType { get; set; }

        public BaseMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new Exception("message cannot be null");

            _message = message;
            _commandNameIndex = AppConfiguration.useSequenceNumber ? 2 : 1;
        }

        public static BaseMessage Create(int device,string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new Exception("message cannot be null");

            string cmdName = GetCommandName(message);

            BaseMessage ret = null;
            var reqType = GetType(device, cmdName, CommandType.Request);

            if (reqType == null)
                return null;

            var resType = GetType(device, cmdName, CommandType.ReplyResponse);
            var eoeType = GetType(device, cmdName, CommandType.ReplyEoE);

            ret = (BaseMessage)Activator.CreateInstance(reqType, new object[] { message });

            var attrib = reqType.GetCustomAttributes(typeof(MessageAttribute), false).FirstOrDefault() as MessageAttribute;
            ret.Type = attrib.MessageType;
            ret.ResponseType = resType;

            if (attrib.MessageType == MessageType.Action || attrib.MessageType == MessageType.Control)
            {
                ret.EndOfExecType = eoeType;
            }

            return ret;
        }

        public static Type GetType(DeviceType deviceType, CommandName commandName, CommandType commandType)
        {
            Type reqType = null;
            Program.MessageLookUp.TryGetValue($"{deviceType}{commandName}{commandType}", out reqType);
            if (reqType == null)
                return null;
            return reqType;
        }

        public static Type GetType(int unit, string command, CommandType commandType)
        {
            var deviceType = (Messages.DeviceType)Enum.Parse(typeof(Messages.DeviceType), unit.ToString());
            var commandName = (CommandName)Enum.Parse(typeof(CommandName), command);
            return GetType(deviceType, commandName,commandType);

        }
        static string GetCommandName(string message)
        {
            if (AppConfiguration.useSequenceNumber)
                return message.Substring(7, 4);
            else
                return message.Substring(4, 4);
        }

        public virtual void Parse()
        {
            string strippeedCmd = string.Empty;

            if (AppConfiguration.checkSumCheck)
                strippeedCmd = _message.Substring(2, _message.Length - 6);
            else
                strippeedCmd = _message.Substring(2, _message.Length - 3);

            _fields = strippeedCmd.Split(',');

            _unitNumber = Convert.ToInt32(_fields[0]);

            if (AppConfiguration.useSequenceNumber)
                _seqNum = Convert.ToInt32(_fields[1]);

            _commandName = _fields[_commandNameIndex];
        }

        public void PreProcess(CommandContext ctxt, Device device)
        {
            if (PerformMessageSpecificPreProcessing(device))
            {
                SendResponse(ctxt, device);
            }
            else
            {
                ctxt.ResponseQCallback(ReceptionError.Generate("2001"));
            }
        }

        public virtual bool PerformMessageSpecificPreProcessing(Device device)
        {
            if (!_commandName.Equals("INIT") && (Type == MessageType.Action || Type == MessageType.Control || Type == MessageType.Setting))
            {
                if (!device.IsError && !device.IsReady)// already in progress. only one of these can be executing at any time, so error out
                    return false;
            }
            return true;
        }
        public void SendResponse(CommandContext ctxt, Device device)
        {
            var reply = (BaseResponse)Activator.CreateInstance(ResponseType, this);

            var res = reply.Generate(device);
            ctxt.ResponseQCallback(res);

            if (Type == MessageType.Action || Type == MessageType.Control)
            {
                device.CommandState = DeviceState.ResponseSent;
                device.IsReady = false;
            }
            //else
            //    device.CommandState = DeviceState.Ready;

            device.PreviousCommand = this;
        }

        public void Process(CommandContext ctxt, Device device)
        {
            //Task.Run(() => 
            //{ 
            if (Type == MessageType.Action || Type == MessageType.Control)
            {
                if (device.CommandState != DeviceState.ResponseSent)
                    return;//TODO raise error

                PerformCommandSpecificProcessing(device);

                SendEndOfExecution(ctxt, device);

                PerformPostEOESend(ctxt, device);
            }
            //});
        }

        public virtual void PerformCommandSpecificProcessing(Device device)
        {

        }

        public void SendEndOfExecution(CommandContext ctxt, Device device)
        {
            var eoe = (BaseEndOfExec)Activator.CreateInstance(EndOfExecType, this);
            var res = eoe.Generate(device);
            ctxt.ResponseQCallback(res);
            device.CommandState = DeviceState.EOESent;
            device.RetryTimer.Start();
            device.PreviousCommand = this;
            device.LastCtxtForWhichSentEoE = new EoEResponseContext(ctxt, res);
        }

        public virtual void PerformPostEOESend(CommandContext ctxt, Device device)
        {

        }
    }
    
    public class EoEResponseContext
    {
        public CommandContext RequestContext;
        public string Response;
        public EoEResponseContext(CommandContext ctxt, string res) => (RequestContext, Response) = (ctxt, res);
    }

    public enum MessageType
    {
        Control, Action, Setting, Reference, Ack
    }

    public enum CommandName
    {
        ACKN,CCLR,CRSM,CSOL,CSRV,CSTP,INIT,MABS,MACA,MALN,MCTR,MMAP,MMCA,MPNT,MREL,MTCH,MTRS,RACA,RALN,RAWC,RCCD,RERR,
        RLOG,RMAP,RMCA,RMPD,RMSK,RPOS,RPRM,RSLV,RSPD,RSTP,RSTR,RSTS,RTRM,RVER,SABS,SAPS,SMSK,SPDL,SPLD,SPOS,SPRM,SPSV,
        SSLV,SSPD,SSTD,SSTR,STRM
    }

    public enum DeviceType
    {
        Manipulator =1,
        PreAligner = 2
    }

    public enum CommandType
    {
        ReplyResponse,
        ReplyEoE,
        Request
    }

    [Flags]
    public enum ResponseStatus1 : short
    {
        None = 0x0,
        UnitReady = 0x2,//1 ready, 0 busy
        ServoOff = 0x4,
        ErrorOccured = 0x8
    }

    [Flags]
    public enum ResponseStatus2 : short
    {
        None = 0x0,
        BattVoltDropped = 0x1,
        Blade1_Vac_Grip_HasWafer = 0x2,//1 ready, 0 busy
        Blade2_LineSensor_Haswafer = 0x4,
    }
}

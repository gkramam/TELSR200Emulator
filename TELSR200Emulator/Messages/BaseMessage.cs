using System;
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

        public static BaseMessage Create(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new Exception("message cannot be null");

            string cmdName = GetCommandName(message);

            int unit = Convert.ToInt32(message.Substring(2, 1));

            if (unit == 1)
                return CreateManipulatorCommand(cmdName, message);
            else
                return CreatePreAlignerCommand(cmdName, message);

        }

        static BaseMessage CreateManipulatorCommand(string cmdName, string message)
        {
            BaseMessage ret = null;
            switch (cmdName)
            {
                //*******************           ACTION       ************************************
                case "INIT":
                    ret = new Manipulator.CommandINIT(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseINIT), EndOfExecType = typeof(Manipulator.EndOfExecINIT) };
                    break;
                case "MTRS":
                    ret = new Manipulator.CommandMTRS(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMTRS), EndOfExecType = typeof(Manipulator.EndOfExecMTRS) };
                    break;
                case "MPNT":
                    ret = new Manipulator.CommandMPNT(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMPNT), EndOfExecType = typeof(Manipulator.EndOfExecMPNT) };
                    break;
                case "MCTR":
                    ret = new Manipulator.CommandMCTR(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMCTR), EndOfExecType = typeof(Manipulator.EndOfExecMCTR) };
                    break;
                case "MTCH":
                    ret = new Manipulator.CommandMTCH(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMTCH), EndOfExecType = typeof(Manipulator.EndOfExecMTCH) };
                    break;
                case "MABS":
                    ret = new Manipulator.CommandMABS(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMABS), EndOfExecType = typeof(Manipulator.EndOfExecMABS) };
                    break;
                case "MREL":
                    ret = new Manipulator.CommandMREL(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMREL), EndOfExecType = typeof(Manipulator.EndOfExecMREL) };
                    break;
                case "MMAP":
                    ret = new Manipulator.CommandMMAP(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMMAP), EndOfExecType = typeof(Manipulator.EndOfExecMMAP) };
                    break;
                case "MMCA":
                    ret = new Manipulator.CommandMMCA(message) { Type = MessageType.Action, ResponseType = typeof(Manipulator.ResponseMMCA), EndOfExecType = typeof(Manipulator.EndOfExecMMCA) };
                    break;
                //*******************           CONTROL       ************************************
                case "CSTP":
                    ret = new Manipulator.CommandCSTP(message) { Type = MessageType.Control, ResponseType = typeof(Manipulator.ResponseCSTP), EndOfExecType = typeof(Manipulator.EndOfExecCSTP) };
                    break;
                case "CRSM":
                    ret = new Manipulator.CommandCRSM(message) { Type = MessageType.Control, ResponseType = typeof(Manipulator.ResponseCRSM), EndOfExecType = typeof(Manipulator.EndOfExecCRSM) };
                    break;
                case "CSRV":
                    ret = new Manipulator.CommandCSRV(message) { Type = MessageType.Control, ResponseType = typeof(Manipulator.ResponseCSRV), EndOfExecType = typeof(Manipulator.EndOfExecCSRV) };
                    break;
                case "CCLR":
                    ret = new Manipulator.CommandCCLR(message) { Type = MessageType.Control, ResponseType = typeof(Manipulator.ResponseCCLR), EndOfExecType = typeof(Manipulator.EndOfExecCCLR) };
                    break;
                case "CSOL":
                    ret = new Manipulator.CommandCSOL(message) { Type = MessageType.Control, ResponseType = typeof(Manipulator.ResponseCSOL), EndOfExecType = typeof(Manipulator.EndOfExecCSOL) };
                    break;
                //*******************           SETTING       ************************************
                case "SSPD":
                    ret = new Manipulator.CommandSSPD(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSSPD) };
                    break;
                case "SSLV":
                    ret = new Manipulator.CommandSSLV(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSSLV) };
                    break;
                case "SPOS":
                    ret = new Manipulator.CommandSPOS(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSPOS) };
                    break;
                case "SABS":
                    ret = new Manipulator.CommandSABS(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSABS) };
                    break;
                case "SAPS":
                    ret = new Manipulator.CommandSAPS(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSAPS) };
                    break;
                case "SPDL":
                    ret = new Manipulator.CommandSPDL(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSPDL) };
                    break;
                case "SPSV":
                    ret = new Manipulator.CommandSPSV(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSPSV) };
                    break;
                case "SPLD":
                    ret = new Manipulator.CommandSPLD(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSPLD) };
                    break;
                case "SSTR":
                    ret = new Manipulator.CommandSSTR(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSSTR) };
                    break;
                case "SPRM":
                    ret = new Manipulator.CommandSPRM(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSPRM) };
                    break;
                case "SMSK":
                    ret = new Manipulator.CommandSMSK(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSMSK) };
                    break;
                case "SSTD":
                    ret = new Manipulator.CommandSSTD(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSSTD) };
                    break;
                case "STRM":
                    ret = new Manipulator.CommandSTRM(message) { Type = MessageType.Setting, ResponseType = typeof(Manipulator.ResponseSTRM) };
                    break;
                //*******************           REFERENCE       ************************************
                case "RSPD":
                    ret = new Manipulator.ReferenceRSPD(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRSPD) };
                    break;
                case "RSLV":
                    ret = new Manipulator.ReferenceRSLV(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRSLV) };
                    break;
                case "RPOS":
                    ret = new Manipulator.ReferenceRPOS(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRPOS) };
                    break;
                case "RSTP":
                    ret = new Manipulator.ReferenceRSTP(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRSTP) };
                    break;
                case "RSTR":
                    ret = new Manipulator.ReferenceRSTR(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRSTR) };
                    break;
                case "RPRM":
                    ret = new Manipulator.ReferenceRPRM(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRPRM) };
                    break;
                case "RSTS":
                    ret = new Manipulator.ReferenceRSTS(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRSTS) };
                    break;
                case "RERR":
                    ret = new Manipulator.ReferenceRERR(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRERR) };
                    break;
                case "RMSK":
                    ret = new Manipulator.ReferenceRMSK(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRMSK) };
                    break;
                case "RVER":
                    ret = new Manipulator.ReferenceRVER(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRVER) };
                    break;
                case "RMAP":
                    ret = new Manipulator.ReferenceRMAP(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRMAP) };
                    break;
                case "RMPD":
                    ret = new Manipulator.ReferenceRMPD(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRMPD) };
                    break;
                case "RMCA":
                    ret = new Manipulator.ReferenceRMCA(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRMCA) };
                    break;
                case "RTRM":
                    ret = new Manipulator.ReferenceRTRM(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRTRM) };
                    break;
                case "RAWC":
                    ret = new Manipulator.ReferenceRAWC(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRAWC) };
                    break;
                case "RLOG":
                    ret = new Manipulator.ReferenceRLOG(message) { Type = MessageType.Reference, ResponseType = typeof(Manipulator.ResponseRLOG) };
                    break;
                //*******************           ACK       ************************************
                case "ACKN":
                    ret = new Manipulator.CommandACKN(message) { Type = MessageType.Ack };
                    break;
            }

            return ret;
        }

        static BaseMessage CreatePreAlignerCommand(string cmdName, string message)
        {
            BaseMessage ret = null;
            switch (cmdName)
            {
                //*******************           ACTION       ************************************
                case "INIT":
                    ret = new PreAligner.CommandINIT(message) { Type = MessageType.Action, ResponseType = typeof(PreAligner.ResponseINIT), EndOfExecType = typeof(PreAligner.EndOfExecINIT) };
                    break;
                case "MABS":
                    ret = new PreAligner.CommandMABS(message) { Type = MessageType.Action, ResponseType = typeof(PreAligner.ResponseMABS), EndOfExecType = typeof(PreAligner.EndOfExecMABS) };
                    break;
                case "MREL":
                    ret = new PreAligner.CommandMREL(message) { Type = MessageType.Action, ResponseType = typeof(PreAligner.ResponseMREL), EndOfExecType = typeof(PreAligner.EndOfExecMREL) };
                    break;
                case "MALN":
                    ret = new PreAligner.CommandMALN(message) { Type = MessageType.Action, ResponseType = typeof(PreAligner.ResponseMALN), EndOfExecType = typeof(PreAligner.EndOfExecMALN) };
                    break;
                case "MACA":
                    ret = new PreAligner.CommandMACA(message) { Type = MessageType.Action, ResponseType = typeof(PreAligner.ResponseMACA), EndOfExecType = typeof(PreAligner.EndOfExecMACA) };
                    break;
                //*******************           CONTROL       ************************************
                case "CSTP":
                    ret = new PreAligner.CommandCSTP(message) { Type = MessageType.Control, ResponseType = typeof(PreAligner.ResponseCSTP), EndOfExecType = typeof(PreAligner.EndOfExecCSTP) };
                    break;
                case "CRSM":
                    ret = new PreAligner.CommandCRSM(message) { Type = MessageType.Control, ResponseType = typeof(PreAligner.ResponseCRSM), EndOfExecType = typeof(PreAligner.EndOfExecCRSM) };
                    break;
                case "CSRV":
                    ret = new PreAligner.CommandCSRV(message) { Type = MessageType.Control, ResponseType = typeof(PreAligner.ResponseCSRV), EndOfExecType = typeof(PreAligner.EndOfExecCSRV) };
                    break;
                case "CCLR":
                    ret = new PreAligner.CommandCCLR(message) { Type = MessageType.Control, ResponseType = typeof(PreAligner.ResponseCCLR), EndOfExecType = typeof(PreAligner.EndOfExecCCLR) };
                    break;
                case "CSOL":
                    ret = new PreAligner.CommandCSOL(message) { Type = MessageType.Control, ResponseType = typeof(PreAligner.ResponseCSOL), EndOfExecType = typeof(PreAligner.EndOfExecCSOL) };
                    break;
                //*******************           SETTING       ************************************
                case "SSPD":
                    ret = new PreAligner.CommandSSPD(message) { Type = MessageType.Setting, ResponseType = typeof(PreAligner.ResponseSSPD) };
                    break;
                case "SSLV":
                    ret = new PreAligner.CommandSSLV(message) { Type = MessageType.Setting, ResponseType = typeof(PreAligner.ResponseSSLV) };
                    break;
                case "SPRM":
                    ret = new PreAligner.CommandSPRM(message) { Type = MessageType.Setting, ResponseType = typeof(PreAligner.ResponseSPRM) };
                    break;
                case "SMSK":
                    ret = new PreAligner.CommandSMSK(message) { Type = MessageType.Setting, ResponseType = typeof(PreAligner.ResponseSMSK) };
                    break;
                case "SSTD":
                    ret = new PreAligner.CommandSSTD(message) { Type = MessageType.Setting, ResponseType = typeof(PreAligner.ResponseSSTD) };
                    break;
                //*******************           REFERENCE       ************************************
                case "RSPD":
                    ret = new PreAligner.ReferenceRSPD(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRSPD) };
                    break;
                case "RSLV":
                    ret = new PreAligner.ReferenceRSLV(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRSLV) };
                    break;
                case "RPOS":
                    ret = new PreAligner.ReferenceRPOS(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRPOS) };
                    break;
                case "RPRM":
                    ret = new PreAligner.ReferenceRPRM(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRPRM) };
                    break;
                case "RSTS":
                    ret = new PreAligner.ReferenceRSTS(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRSTS) };
                    break;
                case "RERR":
                    ret = new PreAligner.ReferenceRERR(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRERR) };
                    break;
                case "RMSK":
                    ret = new PreAligner.ReferenceRMSK(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRMSK) };
                    break;
                case "RVER":
                    ret = new PreAligner.ReferenceRVER(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRVER) };
                    break;
                case "RALN":
                    ret = new PreAligner.ReferenceRALN(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRALN) };
                    break;
                case "RACA":
                    ret = new PreAligner.ReferenceRACA(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRACA) };
                    break;
                case "RCCD":
                    ret = new PreAligner.ReferenceRCCD(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRCCD) };
                    break;
                case "RLOG":
                    ret = new PreAligner.ReferenceRLOG(message) { Type = MessageType.Reference, ResponseType = typeof(ResponseRLOG) };
                    break;
                //*******************           ACKN       ************************************
                case "ACKN":
                    ret = new PreAligner.CommandACKN(message) { Type = MessageType.Ack };
                    break;
            }

            return ret;
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

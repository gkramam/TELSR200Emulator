using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator
{
    public class Device
    {
        public bool Stop { get; set; }

        public bool IsServoOn { get; set; }

        public bool IsBatteryVoltageDropped { get; set; }

        protected BlockingCollection<CommandContext> IncomingQ;

        public bool IsError { get; set; }
        public bool IsReady { get; set; }

        public DeviceState CommandState { get; set; }
        public Device() 
        {
            IncomingQ = new BlockingCollection<CommandContext>();
            CommandState = DeviceState.None;
            IsReady = true;
        }

        public void AddToIncomingQ(CommandContext commandContext)
        {
            IncomingQ.Add(commandContext);
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (!Stop)
                {
                    foreach (var cmdctxt in IncomingQ.GetConsumingEnumerable())
                    {
                        Process(cmdctxt);
                    }
                }
            });
        }

        public BaseMessage commandBeingProcessed, previousCommand;
        public int SeqNum;
        public void Process(CommandContext cmdCxt)
        {
            var cmdstr = cmdCxt.CommandMessage;

            /**
             *   Checksum Verification
             */
            if (AppConfiguration.checkSumCheck)
            {
                var checkSum = cmdstr.Substring(cmdstr.Length - 1 - 2, 2);
                string strippedCmd = cmdstr.Substring(1, cmdstr.Length - 1 - 3);

                if (!CheckSum.Valid(strippedCmd, checkSum))
                {
                    cmdCxt.ResponseQCallback($"Checksum validation failed. Received {cmdstr}");
                    return;
                }
            }

            commandBeingProcessed = BaseMessage.Create(cmdCxt.CommandMessage);
            commandBeingProcessed.Parse();

            /**
             *   Sequence number Verification
             */
            if (AppConfiguration.useSequenceNumber)
            {
                int sn = commandBeingProcessed.SeqNum.Value;

                if (CommandState == DeviceState.None || commandBeingProcessed.CommandName.Equals("INIT"))//first time or reinit
                {
                    SeqNum = sn;
                    //CommandState = DeviceState.Ready;
                }
                else
                {
                    if (sn == SeqNum + 1)
                    {
                        SeqNum = sn;
                    }
                    else if (sn == SeqNum)
                    {
                        CommandState = DeviceState.ErrorSent;
                        cmdCxt.ResponseQCallback($"Duplicate Sequence number. Received {sn}");
                        return;
                    }
                    else
                    {
                        CommandState = DeviceState.ErrorSent;
                        cmdCxt.ResponseQCallback($"Sequence number out of order. Received {sn}");
                        return;
                    }
                }

            }

            if (IsError && !commandBeingProcessed.CommandName.Equals("INIT"))
            {
                CommandState = DeviceState.ErrorSent;
                cmdCxt.ResponseQCallback($"Device in Error state. Rejecting commands");
                return;
            }

            if(CommandState == DeviceState.EOESent && commandBeingProcessed.Type == MessageType.Ack)
            {
                CommandState = DeviceState.Ready;
                IsReady = true;
                return;
            }

            if (!(CommandState == DeviceState.None ||
                CommandState == DeviceState.Ready ||
                CommandState == DeviceState.CommandResponseSent ||
                CommandState == DeviceState.ErrorSent ||
                CommandState == DeviceState.EventSent))
            {
                throw new ApplicationException("Invalid device command state detected");//TODO Raise an error
            }

            commandBeingProcessed.PreProcess(cmdCxt, this);

            if(commandBeingProcessed.Type == MessageType.Action || commandBeingProcessed.Type == MessageType.Control)
                commandBeingProcessed.Process(cmdCxt, this);
        }

        public virtual void Reset()
        {
            IsReady = true;
            IsServoOn = false;
            IsError = false;
            IsBatteryVoltageDropped = false;
            CommandState = DeviceState.Ready;
        }

        public virtual void GoHome(char axesCode)
        {

        }
        public virtual ResponseStatus1 GetResponseStatus1()
        {
            ResponseStatus1 ret = ResponseStatus1.None;
            if (IsReady)
                ret = ret | ResponseStatus1.UnitReady;
            if (IsError)
                ret = ret | ResponseStatus1.ErrorOccured;
            if (!IsServoOn)
                ret = ret | ResponseStatus1.ServoOff;
            return ret;
        }
        public virtual ResponseStatus2 GetResponseStatus2()
        {
            return ResponseStatus2.None;
        }
    }

    public enum DeviceState
    {
        None,
        Ready,
        MessageReceived,
        ResponseSent,
        CommandReceived,
        CommandResponseSent,
        EOESent,
        AcknReceived,
        EventSent,
        ErrorSent
    }
    
}

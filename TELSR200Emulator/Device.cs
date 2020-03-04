﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator
{
    public class Device
    {
        public bool Stop { get; set; }

        private static readonly object _lock = new object();

        private bool _isServoOn;
        public bool IsServoOn 
        {
            get 
            { 
                lock(_lock)
                {
                    return _isServoOn;
                }
            }
            set 
            { 
                lock(_lock)
                {
                    _isServoOn = value;
                }
            } 
        }

        private bool _isBatteryVoltageDropped;
        public bool IsBatteryVoltageDropped 
        {
            get 
            {
                lock(_lock)
                {
                    return _isBatteryVoltageDropped;
                }
            }
            set 
            {
                lock(_lock)
                {
                    _isBatteryVoltageDropped = value;
                }
            } 
        }

        protected BlockingCollection<CommandContext> IncomingQ;

        bool _isError;
        public bool IsError 
        {
            get
            { 
                lock(_lock)
                {
                    return _isError;
                }
            }
            set
            { 
                lock(_lock)
                {
                    _isError = value;
                }
            }
        }

        bool _isReady;
        public bool IsReady 
        {
            get
            {
                lock(_lock)
                {
                    return _isReady;
                }
            }
            set
            { 
                lock(_lock)
                {
                    _isReady = value;
                }
            }
        }

        DeviceState _commandState;
        public DeviceState CommandState 
        {
            get
            {
                lock(_lock)
                {
                    return _commandState;
                }
            }
            set
            { 
                lock(_lock)
                {
                    _commandState = value;
                }
            }
        }

        System.Timers.Timer _retryTimer;
        public System.Timers.Timer RetryTimer 
        {
            get
            {
                lock(_lock)
                {
                    return _retryTimer;
                }
            }
            set
            { 
                lock(_lock)
                {
                    _retryTimer = value;
                }
            }
        }

        EoEResponseContext _lastCtxtForWhichSentEoE;
        public EoEResponseContext LastCtxtForWhichSentEoE 
        {
            get
            { 
                lock(_lock)
                {
                    return _lastCtxtForWhichSentEoE;
                }
            }
            set
            { 
                lock(_lock)
                {
                    _lastCtxtForWhichSentEoE = value;
                }
            }
        }

        BaseMessage _commandBeingProcessed;
        public BaseMessage commandBeingProcessed
        {
            get
            {
                lock (_lock)
                {
                    return _commandBeingProcessed;
                }
            }
            set
            {
                lock (_lock)
                {
                    _commandBeingProcessed = value;
                }
            }
        }
        BaseMessage _previousCommand;
        public BaseMessage PreviousCommand
        {
            get
            {
                lock (_lock)
                {
                    return _previousCommand;
                }
            }
            set
            {
                lock (_lock)
                {
                    _previousCommand = value;
                }
            }
        }
        public int SeqNum;
        int retryCount = 0;

        public Device() 
        {
            IncomingQ = new BlockingCollection<CommandContext>();
            CommandState = DeviceState.None;
            IsReady = true;

            RetryTimer = new System.Timers.Timer(5000);
            RetryTimer.Enabled = false;
            RetryTimer.AutoReset = false;
            RetryTimer.Elapsed += MessageTimer_Elapsed; ;
        }

        private void MessageTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RetryTimer.Stop();
            if (retryCount >= 3)
            {
                return;
            }
            if (CommandState == DeviceState.EOESent)
            {
                if(LastCtxtForWhichSentEoE != null)
                {
                    LastCtxtForWhichSentEoE.RequestContext.ResponseQCallback(LastCtxtForWhichSentEoE.Response);
                    retryCount++;
                    RetryTimer.Start();
                }
            }
            else
                throw new ApplicationException("Invalid Retry control flow detected");
        }

        public void AddToIncomingQ(CommandContext commandContext)
        {
            IncomingQ.Add(commandContext);
        }

        public void Start()
        {
            Task.Run(() =>
            {
                //while (!Stop)
                {
                    foreach (var cmdctxt in IncomingQ.GetConsumingEnumerable())
                    {
                        Process(cmdctxt);
                        if (Stop)
                            break;
                    }
                }
            });
        }

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

                if (!CheckSum.IsValid(strippedCmd, checkSum))
                {
                    cmdCxt.ResponseQCallback(ReceptionError.Generate("2000"));
                    return;
                }
            }

            commandBeingProcessed = BaseMessage.Create(cmdCxt.CommandMessage);
            commandBeingProcessed.Parse();

            /**
             *   Sequence number Verification
             */
            //if (AppConfiguration.useSequenceNumber)
            //{
            //    int sn = commandBeingProcessed.SeqNum.Value;

            //    if (CommandState == DeviceState.None || commandBeingProcessed.CommandName.Equals("INIT"))//first time or reinit
            //    {
            //        SeqNum = sn;
            //        //CommandState = DeviceState.Ready;
            //    }
            //    else
            //    {
            //        if (sn == SeqNum + 1)
            //        {
            //            SeqNum = sn;
            //        }
            //        else if (sn == SeqNum)
            //        {
            //            CommandState = DeviceState.ErrorSent;
            //            cmdCxt.ResponseQCallback($"Duplicate Sequence number. Received {sn}");
            //            return;
            //        }
            //        else
            //        {
            //            CommandState = DeviceState.ErrorSent;
            //            cmdCxt.ResponseQCallback($"Sequence number out of order. Received {sn}");
            //            return;
            //        }
            //    }

            //}

            if (IsError && !commandBeingProcessed.CommandName.Equals("INIT"))
            {
                //cmdCxt.ResponseQCallback(ReceptionError.Generate("2001"));
                //CommandState = DeviceState.ErrorSent;
                return;
            }

            if(commandBeingProcessed.Type == MessageType.Ack)
            {
                if (CommandState == DeviceState.Ready)
                    return;//Duplicate ACK

                while (CommandState != DeviceState.EOESent)
                    Thread.Sleep(0);

                RetryTimer.Stop();
                LastCtxtForWhichSentEoE = null;
                retryCount = 0;
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
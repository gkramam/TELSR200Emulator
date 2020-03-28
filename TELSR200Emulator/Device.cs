using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator
{
    public abstract class Device
    {
        public bool Stop { get; set; }

        private static readonly object _lock = new object();

        private bool _isServoOn;
        public bool IsServoOn
        {
            get
            {
                lock (_lock)
                {
                    return _isServoOn;
                }
            }
            set
            {
                lock (_lock)
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
                lock (_lock)
                {
                    return _isBatteryVoltageDropped;
                }
            }
            set
            {
                lock (_lock)
                {
                    _isBatteryVoltageDropped = value;
                }
            }
        }

        protected BlockingCollection<CommandContext> IncomingQ;
        protected BlockingCollection<Tuple<CommandContext, BaseMessage>> IncomingReferenceQ;
        protected BlockingCollection<Tuple<CommandContext, BaseMessage>> IncomingOtherQ;

        bool _isError;
        public bool IsError
        {
            get
            {
                lock (_lock)
                {
                    return _isError;
                }
            }
            set
            {
                lock (_lock)
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
                lock (_lock)
                {
                    return _isReady;
                }
            }
            set
            {
                lock (_lock)
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
                lock (_lock)
                {
                    return _commandState;
                }
            }
            set
            {
                lock (_lock)
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
                lock (_lock)
                {
                    return _retryTimer;
                }
            }
            set
            {
                lock (_lock)
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
                lock (_lock)
                {
                    return _lastCtxtForWhichSentEoE;
                }
            }
            set
            {
                lock (_lock)
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

        public abstract int UnitNumber { get; }

        public Device()
        {
            IncomingQ = new BlockingCollection<CommandContext>();
            IncomingReferenceQ = new BlockingCollection<Tuple<CommandContext, BaseMessage>>();
            IncomingOtherQ = new BlockingCollection<Tuple<CommandContext, BaseMessage>>();

            CommandState = DeviceState.None;
            IsReady = true;

            RetryTimer = new System.Timers.Timer(AppConfiguration.endOfExecutionRetryTimeout);
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
                if (LastCtxtForWhichSentEoE != null)
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
                foreach (var cmdctxt in IncomingQ.GetConsumingEnumerable())
                {
                    var cmdstr = cmdctxt.CommandMessage;

                    if (AppConfiguration.checkSumCheck)
                    {
                        var checkSum = cmdstr.Substring(cmdstr.Length - 1 - 2, 2);
                        string strippedCmd = cmdstr.Substring(1, cmdstr.Length - 1 - 3);

                        if (!CheckSum.IsValid(strippedCmd, checkSum))
                        {
                            cmdctxt.ResponseQCallback(ReceptionError.Generate("2000"));
                            continue;
                        }
                    }

                    if (!cmdctxt.CommandMessage.Substring(2, 1).Equals(UnitNumber.ToString()))
                    {
                        cmdctxt.ResponseQCallback(ReceptionError.Generate("5000"));
                        continue; ;
                    }

                    var commandBeingCategorized = BaseMessage.Create(UnitNumber,cmdctxt.CommandMessage);

                    if (commandBeingCategorized == null)
                    {
                        cmdctxt.ResponseQCallback(ReceptionError.Generate("6000"));
                        continue; ;
                    }

                    commandBeingCategorized.Parse();//Process(cmdctxt);

                    if (commandBeingCategorized.Type == MessageType.Reference)
                    {
                        IncomingReferenceQ.Add(new Tuple<CommandContext, BaseMessage>(cmdctxt, commandBeingCategorized));
                    }
                    else
                    {
                        IncomingOtherQ.Add(new Tuple<CommandContext, BaseMessage>(cmdctxt, commandBeingCategorized));
                    }

                    if (Stop)
                        break;
                }
            });

            Task.Run(() =>
            {
                foreach (var tuple in IncomingOtherQ.GetConsumingEnumerable())
                {
                    ProcessOther(tuple.Item1, tuple.Item2);
                    if (Stop)
                        break;
                }
            });

            Task.Run(() =>
            {
                foreach (var tuple in IncomingReferenceQ.GetConsumingEnumerable())
                {
                    ProcessReference(tuple.Item1, tuple.Item2);
                    if (Stop)
                        break;
                }
            });
        }

        public void ProcessReference(CommandContext cmdCxt, BaseMessage categorizedCommand)
        {
            categorizedCommand.PreProcess(cmdCxt, this);
        }

        public void ProcessOther(CommandContext cmdCxt, BaseMessage categorizedCommand)
        {
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
            commandBeingProcessed = categorizedCommand;

            if (IsError && !commandBeingProcessed.CommandName.Equals("INIT"))
            {
                cmdCxt.ResponseQCallback(ReceptionError.Generate("2001"));
                CommandState = DeviceState.ErrorSent;
                return;
            }

            if (commandBeingProcessed.Type == MessageType.Ack)
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

            if((CommandState == DeviceState.EOESent || CommandState == DeviceState.CommandResponseSent) && 
                commandBeingProcessed.CommandName.Equals("INIT"))
            {
                RetryTimer.Stop();
                LastCtxtForWhichSentEoE = null;
                retryCount = 0;
                CommandState = DeviceState.Ready;
                IsReady = true;
            }
            else if (!(CommandState == DeviceState.None ||
                CommandState == DeviceState.Ready ||
                CommandState == DeviceState.CommandResponseSent ||
                CommandState == DeviceState.ErrorSent ||
                CommandState == DeviceState.EventSent))
            {
                cmdCxt.ResponseQCallback(ReceptionError.Generate("9001"));
                CommandState = DeviceState.ErrorSent;
                return;
            }

            commandBeingProcessed.PreProcess(cmdCxt, this);

            if (commandBeingProcessed.Type == MessageType.Action || commandBeingProcessed.Type == MessageType.Control)
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

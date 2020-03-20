using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELSR200Emulator.Messages;

namespace TELSR200Emulator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageAttribute: Attribute
    {
        public CommandName CommandName { get; set; }
        public MessageType MessageType { get; set; }
        public CommandType CommandType { get; set; }
        public DeviceType DeviceType { get; set; }

        public MessageAttribute(CommandName commandName,MessageType messageType,CommandType commandType,DeviceType deviceType)
        {
            CommandName = commandName;
            MessageType = messageType;
            CommandType = commandType;
            DeviceType = deviceType;
        }

    }


}

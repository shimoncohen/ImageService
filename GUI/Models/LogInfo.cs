using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI;

namespace GUI.Models
{
    class LogInfo
    {
        private string m_Message;
        public string Message
        {
            get { return m_Message; }
            set
            {
                m_Message = value;
            }
        }
        private MessageTypeEnum m_MessageTypeEnum;
        public MessageTypeEnum Type
        {
            get { return m_MessageTypeEnum; }
            set
            {
                m_MessageTypeEnum = value;
            }
        }

        public string ColorList()
        {
            switch(this.m_MessageTypeEnum)
            {
                case MessageTypeEnum.INFO:
                    return "Green";
                case MessageTypeEnum.WARNING:
                    return "Yellow";
                case MessageTypeEnum.FAIL:
                    return "Red";
                default:
                    return "Transparent";
            }
        }

        public LogInfo(MessageTypeEnum MessageTypeEnum, string Message)
        {
            this.m_MessageTypeEnum = MessageTypeEnum;
            this.m_Message = Message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI;

namespace GUI.Models
{
    class LogInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string m_Message;
        public string Message
        {
            get { return m_Message; }
            set
            {
                m_Message = value;
                OnPropertyChanged("Message");
            }
        }

        private string m_MessageType;
        //private MessageTypeEnum m_MessageTypeEnum;
        public string Type
        {
            get
             {
                return this.m_MessageType;
                 /*switch(m_MessageTypeEnum)
                 {
                     case MessageTypeEnum.INFO:
                         return "INFO";
                     case MessageTypeEnum.WARNING:
                         return "WARNING";
                     case MessageTypeEnum.FAIL:
                         return "FAIL";
                     default:
                         return "";
                 }*/

             }

             set
             {
                this.m_MessageType = value;
                OnPropertyChanged("Type");
                /*switch (value)
                {
                    case "INFO":
                        m_MessageTypeEnum = MessageTypeEnum.INFO;
                        break;
                    case "WARNING":
                        m_MessageTypeEnum = MessageTypeEnum.WARNING;
                        break;
                    case "FAIL":
                        m_MessageTypeEnum = MessageTypeEnum.FAIL;
                        break;
                    default:
                        m_MessageTypeEnum = MessageTypeEnum.FAIL;
                        break;
                }*/
            }
        }

        public string ColorList()
        {
            switch (this.m_MessageType)
            {
                case "INFO":
                    return "Green";
                case "WARNING":
                    return "Yellow";
                case "FAIL":
                    return "Red";
                default:
                    return "Transparent";
            }
            /*switch (this.m_MessageTypeEnum)
            {
                case MessageTypeEnum.INFO:
                    return "Green";
                case MessageTypeEnum.WARNING:
                    return "Yellow";
                case MessageTypeEnum.FAIL:
                    return "Red";
                default:
                    return "Transparent";
            }*/
        }

        public LogInfo()
        {
        }

        /*public LogInfo(MessageTypeEnum MessageTypeEnum, string Message)
        {
            this.m_MessageTypeEnum = MessageTypeEnum;
            this.m_Message = Message;
        }*/

        public LogInfo(string MessageType, string Message)
        {
            this.m_MessageType = MessageType;
            this.m_Message = Message;
        }
    }
}

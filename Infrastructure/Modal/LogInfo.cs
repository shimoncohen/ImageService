using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Enums;

namespace Infrastructure.Modal
{
    /// <summary>
    /// the class gets args containing message and status, and parse them.
    /// </summary>
    public class LogInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string m_Message;
        /// <summary>
        /// getter and setter of the status
        /// </summary>
        /// <return> returns the status of the operation </return>
        public string Message { get; set; }

        private MessageTypeEnum m_MessageType;
        /// <summary>
        /// getter and setter of the message
        /// </summary>
        /// <return> returns the message of the logger </return>
        public MessageTypeEnum Status { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public LogInfo(MessageTypeEnum MessageType, string Message)
        {
            this.m_MessageType = MessageType;
            this.m_Message = Message;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogInfo() { }
    }
}

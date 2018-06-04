using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Logs
    {
        private string filter;
        [Required]
        [Display(Name = "Status")]
        private string Status;
        [Required]
        [Display(Name = "Message")]
        private string Message;
        public Logs(string Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
            m_Connection = Communication.CreateConnectionChannel();

            SendInfo += m_Connection.StartSenderChannel;
            // sign to the event of getting the info from the server
            m_Connection.InfoRecieved += GetInfoFromServer;
            System.Threading.Thread.Sleep(50);
            SendToServer();
        }

        public string Filter {
            set
            {
                this.filter = value;
            }
        }

        public string GetMessage {
            get
            {
                if (this.Status.Equals(filter) || this.filter == null)
                    return this.Message;
                return "";
            }
        }

        public string GetStatus
        {
            get
            {
                if (this.Status.Equals(filter) || this.filter == null)
                    return this.Status;
                return "";
            }
        }
    }
}
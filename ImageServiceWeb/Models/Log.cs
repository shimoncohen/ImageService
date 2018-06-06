using Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    public class Log
    {
        public string GetMessage { get { return this.Message; } }

        public MessageTypeEnum GetStatus { get { return this.Status; } }

        [Required]
        [Display(Name = "Status")]
        private MessageTypeEnum Status;
        [Required]
        [Display(Name = "Message")]
        private string Message;

        public Log(string Status, string Message)
        {
            switch (Status)
            {
                case "info":
                    this.Status = MessageTypeEnum.INFO;
                    break;
                case "warning":
                    this.Status = MessageTypeEnum.WARNING;
                    break;
                case "fail":
                    this.Status = MessageTypeEnum.FAIL;
                    break;
            }
            this.Message = Message;
        }
    }
}
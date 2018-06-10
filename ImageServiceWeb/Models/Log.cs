using Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// The class for the log
    /// </summary>
    public class Log
    {
        /// <summary>
        /// getter for the message
        /// </summary>
        public string GetMessage { get { return this.Message; } }

        /// <summary>
        /// getter for the status
        /// </summary>
        public MessageTypeEnum GetStatus { get { return this.Status; } }

        [Required]
        [Display(Name = "Status")]
        private MessageTypeEnum Status;

        [Required]
        [Display(Name = "Message")]
        private string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="Status">The status.</param>
        /// <param name="Message">The message.</param>
        public Log(string Status, string Message)
        {
            switch (Status.ToLower())
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
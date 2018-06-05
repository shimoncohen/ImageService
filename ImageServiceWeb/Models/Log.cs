using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    public class Log
    {
        public string GetMessage { get { return this.Message; } }

        public string GetStatus { get { return this.Status; } }

        [Required]
        [Display(Name = "Status")]
        private string Status;
        [Required]
        [Display(Name = "Message")]
        private string Message;

        public Log(string Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }
    }
}
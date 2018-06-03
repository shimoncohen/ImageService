using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Logs
    {
        string Status, Message;

        public Logs(string Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public string LogsInfo { get; }
    }
}
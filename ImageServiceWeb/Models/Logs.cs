using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Logs
    {
        private string Status;
        private string Message;
        public Logs(string Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }

        public string LogsInfo { get; }
    }
}
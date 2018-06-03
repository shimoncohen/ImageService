﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Logs
    {
        private string filter;
        private string Status;
        private string Message;
        public Logs(string Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }

        public string Filter {
            set
            {
                this.filter = value;
            }
        }

        public string LogsInfo {
            get
            {
                if (this.Status.Equals(filter) || this.filter == null)
                    return this.Status+" | "+this.Message;
                return "";
            }
        }
    }
}
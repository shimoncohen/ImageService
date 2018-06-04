using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class LogsModel
    {
        private string filter = null;
        private List<Logs> logs = new List<Logs>()
        {
            {new Logs("Info", "Message1") },
            {new Logs("Warning", "Message2") },
            {new Logs("Failed", "Message3") }
        };

        public LogsModel() { }

        public List<Logs> LogsList {
            get {
                List<Logs> filteredList = new List<Logs>();
                foreach (Logs log in logs)
                {
                    if (String.IsNullOrEmpty(this.filter) || this.filter.Equals(log.GetStatus))
                        filteredList.Add(log);
                }
                return filteredList;
            }
        }

        public string SetFilter
        {
            set
            {
                this.filter = value;
            }
        }
    }
}
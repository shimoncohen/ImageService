using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class LogsModel
    {
        private string filter = null;
        private List<Log> logs = new List<Log>()
        {
            {new Log("Info", "Message1") },
            {new Log("Warning", "Message2") },
            {new Log("Failed", "Message3") }
        };

        public LogsModel() { }

        public List<Log> LogsList {
            get {
                List<Log> filteredList = new List<Log>();
                foreach (Log log in logs)
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
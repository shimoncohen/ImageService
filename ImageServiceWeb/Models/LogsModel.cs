using ImageServiceWeb.Connection;
using Infrastructure.Enums;
using Infrastructure.Modal;
using Infrastructure.Modal.Event;
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

        private Communication m_Connection;

        public event EventHandler<CommandRecievedEventArgs> SendInfo;

        public LogsModel()
        {
            m_Connection = Communication.CreateConnectionChannel();

            SendInfo += m_Connection.StartSenderChannel;
            // sign to the event of getting the info from the server
            m_Connection.InfoRecieved += GetInfoFromServer;
            System.Threading.Thread.Sleep(50);
            SendToServer();
        }

        /// <summary>
        /// The function sends a log command to the server.
        /// </summary>
        public void SendToServer()
        {
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, args, "Empty");
            SendInfo?.Invoke(this, e);
        }

        public void GetInfoFromServer(object sender, InfoEventArgs e)
        {
            int infoType = InfoReceivedParser.parseInfoType(e.InfoId);
            // if infoType is 2 it is an information for the logs model
            if (infoType == 2)
            {
                // 1 is to get a new log from the server
                if (e.InfoId == 1)
                {
                    this.AddNewLog(e);
                }
                else if (e.InfoId == 2)  // 2 is to get log history
                {
                    this.SetLogHistory(e);
                }
            }
        }

        /// <summary>
        /// The function adds a log to the list of logs.
        /// </summary>
        /// <param name="e">The log we add to the list</param>
        public void AddNewLog(InfoEventArgs e)
        {
            string[] answer = e.Args;
            // split the received log by ",".
            // log[1] is the message 
            string message = answer[1];
            // log[0] is the message type
            MessageTypeEnum type = ParseTypeFromString(answer[0]);
            LogInfo m = new LogInfo() { Status = type, Message = message };
            // add to the logs list
            Application.Current.Dispatcher.Invoke(new Action(() => { m_LogsInfoList.Add(m); }));
            OnPropertyChanged("LogsInfoList");
        }

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
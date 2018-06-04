using ImageServiceWeb.Connection;
using Infrastructure.Enums;
using Infrastructure.Modal;
using Infrastructure.Modal.Event;
using System;
using System.Collections.Generic;
using System.Windows;
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
        /// The function adds all the history of the log to the logs list.
        /// </summary>
        /// <param name="e">The logs from the logger as an array of strings</param>
        public void SetLogHistory(InfoEventArgs e)
        {
            string[] answer = e.Args;
            for (int i = 0; i < answer.Length; i++)
            {
                // split the received log by ",".
                string[] log = answer[i].Split(',');
                // log[1] is the message 
                string message = log[1];
                // log[0] is the message type
                Log m = new Log(log[0], message);
                //LogInfo m = new LogInfo() { Status = type, Message = message };
                // add to the logs list
                //Application.Current.Dispatcher.Invoke(new Action(() => { m_LogsInfoList.Add(m); }));
                logs.Add(m);
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
            // answer[1] is the message 
            string message = answer[1];
            // answer[0] is the message type
            Log m = new Log(answer[0], message);
            // add to the logs list
            //Application.Current.Dispatcher.Invoke(new Action(() => { logs.Add(m); }));
            logs.Add(m);
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
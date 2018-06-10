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

    /// <summary>
    /// The class of the Logs model
    /// </summary>
    public class LogsModel
    {
        // members
        private static List<Log> logs = new List<Log>();
        private bool gotHistory;
        private object locker;

        /// <summary>
        /// Getter and setter to filter
        /// </summary>
        public string Filter { get; set; }

        private Communication m_Connection;

        public event EventHandler<CommandRecievedEventArgs> SendInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        public LogsModel()
        {
            m_Connection = Communication.CreateConnectionChannel();
            gotHistory = false;
            // mutex
            locker = new object();
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

        /// <summary>
        /// The function gets the logs from the server. 
        /// depends on the type of the command it adds a new log or adds the whole history of logs
        /// <param name="e">The arguments sent from the server</param>
        /// <paramref name="sender">The sender object</paramref>
        /// </summary>
        public void GetInfoFromServer(object sender, InfoEventArgs e)
        {
            int infoType = InfoReceivedParser.parseInfoType(e.InfoId);
            // if infoType is 2 it is an information for the logs model
            if (infoType == 2)
            {
                // 1 is to get a new log from the server
                if (e.InfoId == 1)
                {
                    lock(locker)
                    {
                        this.AddNewLog(e);
                    }
                }
                else if (!gotHistory && e.InfoId == 2)  // 2 is to get log history
                {
                    lock(locker)
                    {
                        logs.Clear();
                        this.SetLogHistory(e);
                    }
                    gotHistory = true;
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
                // add to the logs list
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
            logs.Add(m);
        }

        /// <summary>
        /// getter for the list of logs
        /// </summary>
        public List<Log> LogsList {
            get {
                // create the filterd list
                List<Log> filteredList = new List<Log>();
                List<Log> temp;
                lock (locker)
                {
                    temp = new List<Log>(logs);
                }
                string currFilter = this.Filter;
                // for each log in the list of logs check if it matchs the filter
                foreach (Log log in temp)
                {
                    MessageTypeEnum type = log.GetStatus;
                    if (String.IsNullOrEmpty(currFilter) || this.Filter.Equals(EnumTranslator.MessageTypeToString(log.GetStatus)))
                    {
                        filteredList.Add(log);
                    }
                    else if (String.IsNullOrEmpty(currFilter) || log.GetMessage.Contains(currFilter))
                    {
                        filteredList.Add(log);
                    }
                }
                Filter = "";
                return filteredList;
            }
        }
    }
}
﻿using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class LogHistory
    {
        public List<string[]> Logs { get; }

        private static LogHistory logHistory;

        private LogHistory()
        {
            Logs = new List<string[]>();
        }

        /// <summary>
        /// request a new instance of the class.
        /// </summary>
        /// <return> returns the instance of the object (singleton) </return>
        public static LogHistory CreateLogHistory()
        {
            // if not already created
            if (logHistory == null)
            {
                logHistory = new LogHistory();
            }
            // otherwise create new instance
            return logHistory;
        }

        public void UpdateLog(object sender, MessageRecievedEventArgs e)
        {
            string[] log = new string[2];
            log[0] = e.Status.ToString();
            log[1] = e.Message;
            Logs.Add(log);
        }

        public void ResetLog()
        {
            Logs.Clear();
        }
    }
}
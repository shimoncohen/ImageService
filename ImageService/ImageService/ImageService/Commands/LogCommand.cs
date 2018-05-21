using ImageService.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;
using Infrastructure.Enums;
using Infrastructure.Modal.Event;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {

        /// <summary>
        /// the function executes the get log command
        /// </summary>
        /// <param name= args> the commands arguments </param>
        /// <param name= result> the result of the function, if succeeded or not </param>
        /// <return> returns the log history as a string </return>
        public string Execute(string[] args, out bool result)
        {
            LogHistory logHistory = LogHistory.CreateLogHistory();
            result = true;
            List<string[]> logList = logHistory.Logs;
            // the log array to return, each cell has the message type comma message
            string[] answer = new string[logList.Count];
            int i = 0;
            // create an array of strings representing the log
            foreach (string[] log in logList.ToArray())
            {
                // log[0] is the message type, log[1] is the message itself
                answer[i] = log[0] + "," + log[1];
                i++;
            }
            // return the info converted to Json ready to be sent to client
            InfoEventArgs info = new InfoEventArgs((int)EnumTranslator.CommandToInfo((int)CommandEnum.LogCommand), answer);
            return JsonConvert.SerializeObject(info);
        }
    }
}

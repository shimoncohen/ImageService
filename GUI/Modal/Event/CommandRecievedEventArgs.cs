using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Modal.Event
{
    /// <summary>
    /// the class is in charge of the command received event
    /// </summary>
    public class CommandRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// getter and setter to the command id
        /// </summary>
        /// <return> returns the command id </return>
        public int CommandID { get; set; }      // The Command ID
        /// <summary>
        /// getter and setter to the args
        /// </summary>
        public string[] Args { get; set; }
        /// <summary>
        /// getter and setter to the request dir path
        /// </summary>
        /// <return> returns the path to the requested directory </return>
        public string RequestDirPath { get; set; }  // The Request Directory

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name= id> the id of the event </param>
        /// <param name= args> the args we receive </param>
        /// <param name= path> the path to the directory on which the command will execute </param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}

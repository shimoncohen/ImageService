using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal.Event
{
    /// <summary>
    /// the class that is in charge of the directory close event
    /// </summary>
    public class DirectoryCloseEventArgs : EventArgs
    {
        /// <summary>
        /// getter and setter to the directory path.
        /// </summary>
        /// <return> returns the path to the directory </return>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// getter and setter to the message
        /// </summary>
        public string Message { get; set; }             // The Message That goes to the logger

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name= dirPath> the path to the directory we want to close </param>
        /// <param name= message> the message we write to the logger </param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }

    }
}

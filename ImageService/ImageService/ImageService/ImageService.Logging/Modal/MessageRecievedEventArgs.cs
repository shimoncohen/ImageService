using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// the class gets args containing message and status, and parse them.
    /// </summary>
    public class MessageRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// getter and setter of the status
        /// </summary>
        /// <return> returns the status of the operation </return>
        public MessageTypeEnum Status { get; set; }
        /// <summary>
        /// getter and setter of the message
        /// </summary>
        /// <return> returns the message of the logger </return>
        public string Message { get; set; }
    }
}

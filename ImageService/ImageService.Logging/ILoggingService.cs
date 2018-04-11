using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// an interface for logger of the service.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// an event that handles messages that aare being recieved to the loggger.
        /// </summary>
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// the function recieves a message and invoke the logger recieving mechanism (to write down the message).
        /// </summary>
        /// <param>
        /// a message to the logger, and the type of message (fail, info etc...)
        /// </param>
        void Log(string message, MessageTypeEnum type);           // Logging the Message
    }
}

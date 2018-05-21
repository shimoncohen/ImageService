using Infrastructure.Enums;
using Infrastructure.Modal.Event;
using System;

namespace ImageService.Logging
{
    /// <summary>
    /// an interface for logger of the service.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// an event that handles messages that are being recieved to the loggger.
        /// </summary>
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// an event that handles messages to be sent to all clients.
        /// </summary>
        event EventHandler<InfoEventArgs> NotifyClients;
        /// <summary>
        /// the function recieves a message and invoke the logger recieving mechanism (to write down the message).
        /// </summary>
        /// <param>
        /// a message to the logger, and the type of message (fail, info etc...)
        /// </param>
        void Log(string message, MessageTypeEnum type);           // Logging the Message
    }
}

using ImageService.Logging.Modal;
using System;

namespace ImageService.Logging
{
    /// <summary>
    /// the class that is in charge of the logging operation
    /// </summary>
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs msg = new MessageRecievedEventArgs();
            msg.Message = message;
            msg.Status = type;
            MessageRecieved?.Invoke(this, msg);
        }
    }
}

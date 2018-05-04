using ImageService.Logging.Modal;
using Infrastructure.Enums;
using Infrastructure.Modal.Event;
using System;

namespace ImageService.Logging
{
    /// <summary>
    /// the class that is in charge of the logging operation
    /// </summary>
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public event EventHandler<InfoEventArgs> NotifyClients;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs msg = new MessageRecievedEventArgs();
            msg.Message = message;
            msg.Status = type;
            MessageRecieved?.Invoke(this, msg);
            string[] args = new string[2];
            args[0] = type.ToString();
            args[1] = message;
            InfoEventArgs info = new InfoEventArgs((int)InfoEnums.LogInfo, args);
            NotifyClients?.Invoke(this, info);
        }
    }
}

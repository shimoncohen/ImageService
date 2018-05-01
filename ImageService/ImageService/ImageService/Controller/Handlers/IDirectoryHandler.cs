using ImageService.Modal;
using Infrastructure.Modal.Event;
using System;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// the interface of the directory handler
    /// </summary>
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        /// <summary>
        /// the function starts the handling of a directory
        /// </summary>
        /// <param name= dirPath> the path of the directory </param>
        void StartHandleDirectory(string dirPath);             // The Function Recieves the directory to Handle
        /// <summary>
        /// the function runs when command is received.
        /// </summary>
        /// <param name= sender> the sender object </param>
        /// <param name= e> the event that received the command </param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // The Event that will be activated upon new Command
    }
}

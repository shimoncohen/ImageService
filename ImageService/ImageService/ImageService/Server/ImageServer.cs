using ImageService.Commands;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

namespace ImageService.Server
{
    /// <summary>
    /// the class of the server. holds a controller to execute commands and a logger to write operations that occured.
    /// </summary>
    public class ImageServer
    {
        #region Members
        private IImageController controller;
        private ILoggingService logging;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        //public event EventHandler<DirectoryCloseEventArgs> Closing;
        #endregion

        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name= model> the image modal we have, to create the proper controller. </param>
        /// <param name= handler> the paths to all the directories we want to monitor </param>
        /// <param name= logger> a logger to follow action and operations that occured </param>
        public ImageServer(IImageServiceModal model, string[] handlers, ILoggingService logger)
        {
            // create controller
            this.controller = new ImageController(model);
            this.logging = logger;
            // create handler for each given directory
            foreach (string directory in handlers)
            {
                CreateHandler(directory);
            }
        }
 
        /// <summary>
        /// creates a new handler for a given directory
        /// </summary>
        /// <param name= directory> the path to the directory we want to monitor </param>
        private void CreateHandler(string directory)
        {
            // create handler for given directory
            IDirectoryHandler directoryHandler = new DirectoyHandler(this.controller, this.logging);
            directoryHandler.DirectoryClose += new EventHandler<DirectoryCloseEventArgs>(CloseHandler);
            this.CommandRecieved += directoryHandler.OnCommandRecieved;
            directoryHandler.StartHandleDirectory(directory);
        }

        /// <summary>
        /// the function stops the handling of a specific directiory
        /// </summary>
        /// <param name= sender> the object that sent the request </param>
        /// <param name= e> the event that occured </param>
        private void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler handlerToClose = (IDirectoryHandler)sender;
            this.CommandRecieved -= handlerToClose.OnCommandRecieved;
            this.logging.Log("closed handler for " + e.DirectoryPath, MessageTypeEnum.INFO);
            //handlerToClose.StopHandleDirectory();
            // delete handler
        }

        /// <summary>
        /// the function closes the server. invoke closing operation and writes to log.
        /// </summary>
        public void CloseServer()
        {
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, "*");
            this.CommandRecieved?.Invoke(this, e);
            this.logging.Log("Server closing", MessageTypeEnum.INFO);
        }
    }
}
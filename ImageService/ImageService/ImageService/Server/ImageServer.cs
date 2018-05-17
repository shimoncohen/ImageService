using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using System;
using ImageService.Logging.Modal;
using ImageService.Logging.Modal.Event;
using ImageService.Infrastructure.Enums;
using System.Collections.Generic;

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
        private List<IDirectoryHandler> handlerList;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        // The event that notifies about new info
        public event EventHandler<InfoEventArgs> NotifyClients;
        #endregion

        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name= model> the image modal we have, to create the proper controller. </param>
        /// <param name= handler> the paths to all the directories we want to monitor </param>
        /// <param name= logger> a logger to follow action and operations that occured </param>
        public ImageServer(ImageController imageController, string[] handlers, ILoggingService logger)
        {
            // create controller
            controller = imageController;
            logging = logger;
            handlerList = new List<IDirectoryHandler>();
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
            handlerList.Add(directoryHandler);
            directoryHandler.DirectoryClose += new EventHandler<DirectoryCloseEventArgs>(CloseHandler);
            this.CommandRecieved += directoryHandler.OnCommandRecieved;
            directoryHandler.StartHandleDirectory(directory);
        }

        public List<IDirectoryHandler> getHandlers()
        {
            return handlerList;
        }

        public void NewCommand(object sender, CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(this, e);
        }

        /// <summary>
        /// the function stops the handling of a specific directiory
        /// </summary>
        /// <param name= sender> the object that sent the request </param>
        /// <param name= e> the event that occured </param>
        public void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            List<IDirectoryHandler> list = getHandlers();
            foreach(IDirectoryHandler handler in list)
            {
                if(e.DirectoryPath.Equals("*") || handler.GetPath().Equals(e.DirectoryPath))
                {
                    this.CommandRecieved -= handler.OnCommandRecieved;
                    this.logging.Log("Closed handler for " + e.DirectoryPath, MessageTypeEnum.INFO);

                    //TODO: check if handler is deleted
                    //handlerToClose.DirectoryClose -= CloseHandler;
                    // delete handler
                    handler.StopHandleDirectory(e.DirectoryPath);
                    string path = e.DirectoryPath;
                    string[] args = { path };
                    // create info args
                    InfoEventArgs info = new InfoEventArgs((int)InfoEnums.CloseHandlerInfo, args);
                    // remove the handler from the app config handler list
                    ServiceInfo serviceInfo = ServiceInfo.CreateServiceInfo();
                    serviceInfo.RemoveHandler(e.DirectoryPath);
                    // notify all of the clients that the handler was closed
                    NotifyClients?.Invoke(this, info);
                }
            }
        }

        /// <summary>
        /// the function closes the server. invoke closing operation and writes to log.
        /// </summary>
        public void CloseServer()
        {
            string[] args = { "*" };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, "*");
            this.CommandRecieved?.Invoke(this, e);
            this.logging.Log("Server closing", MessageTypeEnum.INFO);
        }
    }
}
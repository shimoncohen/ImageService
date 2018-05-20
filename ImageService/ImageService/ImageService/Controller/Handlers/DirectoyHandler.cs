using System;
using System.IO;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Logging.Modal.Event;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// the class that handles directories
    /// </summary>
    public class DirectoyHandler : IDirectoryHandler
    {
        private String directoryPath;
        private IImageController imageController;
        private ILoggingService loggingModal;
        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        private readonly String[] fileTypes = { ".jpg", ".png", ".gif", ".bmp" };
        //private List<FileSystemWatcher> watchers;
        private FileSystemWatcher watcher;

        public DirectoyHandler(IImageController controller, ILoggingService service)
        {
            this.imageController = controller;
            this.loggingModal = service;
        }

        public void StartHandleDirectory(string dirPath)
        {
            this.directoryPath = dirPath;
            this.watcher = new FileSystemWatcher(dirPath, "*");
            this.watcher.EnableRaisingEvents = true;
            this.watcher.Created += new FileSystemEventHandler(this.NewFile);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (this.directoryPath.Equals(e.RequestDirPath) || e.RequestDirPath.Equals("*"))
            {
                bool result;
                // execute recieved command
                string message = this.imageController.ExecuteCommand(e.CommandID, e.Args, out result);
                // check if command has executed succesfully and write result to the log
                if (result)
                {
                    loggingModal.Log(message, MessageTypeEnum.INFO);
                }
                else
                {
                    loggingModal.Log(message, MessageTypeEnum.FAIL);
                }
            }
        }

        /// <summary>
        /// the function parse the information of the new file we want to create and calls OnCommandRecieved
        /// </summary>
        /// <param name= sender> the sender object </param>
        /// <param name= e> the event that called the function (holds information of the file) </param>
        private void NewFile(object sender, FileSystemEventArgs e)
        {
            foreach (string extention in this.fileTypes) {
                if(Path.GetExtension(e.FullPath) == extention) {
                    String[] args = { e.FullPath, e.Name };
                    CommandRecievedEventArgs temp = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                        args, this.directoryPath);
                    this.OnCommandRecieved(this, temp);
                    return;
                }
            }
        }
        /// <summary>
        /// the function stops the handling of a directory
        /// </summary>
        public void StopHandleDirectory(string path)
        {
            if (path == this.directoryPath)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
        }

        public string GetPath()
        {
            return this.directoryPath;
        }
    }
}
using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

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
        private readonly String[] fileTypes = { "*.jpg", "*.png", "*.gif", "*.bmp" };
        private List<FileSystemWatcher> watchers;

        public DirectoyHandler(IImageController controller, ILoggingService service)
        {
            this.imageController = controller;
            this.loggingModal = service;
            this.watchers = new List<FileSystemWatcher>();
        }

        public void StartHandleDirectory(string dirPath)
        {
            this.directoryPath = dirPath;
            // create filesystem watchers for every file
            for (int i = 0; i < this.fileTypes.Length; i++)
            {
                FileSystemWatcher fw = new FileSystemWatcher(dirPath, this.fileTypes[i]);
                fw.EnableRaisingEvents = true;
                fw.Created += new FileSystemEventHandler(this.NewFile);
                this.watchers.Add(fw);
            }
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
            String[] args = { e.FullPath, e.Name };
            CommandRecievedEventArgs temp = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                args, this.directoryPath);
            //this.imgC.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
            this.OnCommandRecieved(this, temp);
        }

        public void StopHandleDirectory(object sender, DirectoryCloseEventArgs e)
        {
            // dispose of all file watchers in handler
            /*foreach (FileSystemWatcher watcher in this.watchers) {
                watcher.Dispose();
            }*/
            if(this.directoryPath.Equals(e.DirectoryPath) || e.DirectoryPath.Equals("*")) 
            {
                this.watchers.Clear();
                this.loggingModal.Log(e.Message, MessageTypeEnum.INFO);
            }
        }
    }
}
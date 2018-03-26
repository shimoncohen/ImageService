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
    public class DirectoyHandler : IDirectoryHandler
    {
        private String directoryPath;
        private IImageController imageController;
        private FileSystemWatcher fileSystemWatcher;
        private ILoggingService loggingModal;
        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        private readonly String[] fileTypes = { "*.jpg", "*.png", "*.gif", "*.bmp" };
        private List<FileSystemWatcher> watchers;

        public DirectoyHandler(IImageController controller/*, FileSystemWatcher watcher,
            ILoggingService service*/)
        {
            this.imageController = controller;
            //this.fileSystemWatcher = watcher;
            //this.loggingModal = service;
            this.watchers = new List<FileSystemWatcher>();
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (this.directoryPath.Equals(e.RequestDirPath))
            {
                bool result;
                // execute recieved command
                String message = imageController.ExecuteCommand(e.CommandID, e.Args, out result);
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

        public void StartHandleDirectory(string dirPath)
        {
            this.directoryPath = dirPath;
            // create filesystem watchers for every file
            for(int i = 0; i < 4; i++)
            {
                FileSystemWatcher fw = new FileSystemWatcher(dirPath, this.fileTypes[i]);
                fw.EnableRaisingEvents = true;
                fw.Created += new FileSystemEventHandler(NewFile);
                this.watchers.Add(fw);
            }
        }

        private void NewFile(object sender, FileSystemEventArgs e)
        {
            String[] args = { e.FullPath };
            CommandRecievedEventArgs temp = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                args, this.directoryPath);
            //this.imgC.ExecuteCommand((int)CommandEnum.NewFileCommand, args, out result);
            this.OnCommandRecieved(this, temp);
        }
    }
}
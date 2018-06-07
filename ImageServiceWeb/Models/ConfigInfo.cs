using ImageServiceWeb.Connection;
using ImageServiceWeb.WebEventArgs;
using Infrastructure.Enums;
using Infrastructure.Modal.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigInfo
    {
        public bool InfoReceived { private set; get; }
        //private readonly List<string> handlers;
        [Required]
        [Display(Name = "Handlers")]
        public List<DirectoryModel> Handlers { get; set; }
        //private readonly string outputDir;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }
        //private readonly string sourceName;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }
        //private readonly string logName;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }
        //private readonly int thumbnailSize;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public string ThumbnailSize { get; set; }

        private Communication m_Connection;

        public event EventHandler<CommandRecievedEventArgs> SendInfo;
        public event EventHandler<PhotosEventArgs> sendPath;

        public ConfigInfo()
        {
            Handlers = new List<DirectoryModel>();
            /*foreach (string dir in handlers)
            {
                Handlers.Add(new DirectoryModel(dir));
            }
            OutputDir = outputDir;
            SourceName = sourceName;
            LogName = logName;
            ThumbnailSize = thumbnailSize.ToString();*/

            m_Connection = Communication.CreateConnectionChannel();
            // sign to the event of receive info from the server
            m_Connection.InfoRecieved += GetInfoFromServer;
            SendInfo += m_Connection.StartSenderChannel;
            System.Threading.Thread.Sleep(50);
            // initialize the fields
            this.SendCommandToServer(CommandEnum.GetConfigCommand, "");
        }

        /// <summary>
        /// The function adds a directory to the handlers list
        /// </summary>
        /// <param name="s">The path of the directory</param>
        public void AddToHandlersList(string s)
        {
            DirectoryModel dir = new DirectoryModel(s);
            Handlers.Add(dir);
        }

        /// <summary>
        /// The function removes a directory from the list of directories we listen to.
        /// </summary>
        /// <param name="e">The path of the directory we want to remove</param>
        public void RemoveFromHandlersList(InfoEventArgs e)
        {
            string[] args = e.Args;
            // get the path of the directory
            string dirToRemove = args[0];
            foreach(DirectoryModel dir in Handlers)
            {
                if(dir.DirPath.Equals(dirToRemove))
                {
                    Handlers.Remove(dir);
                    break;
                }
            }
        }

        /// <summary>
        /// The function receives the info from the server and update the model's fields accordingly.
        /// </summary>
        public void GetInfoFromServer(object sender, InfoEventArgs e)
        {
            // parse the info from the infoEventArgs
            int infoType = InfoReceivedParser.parseInfoType(e.InfoId);
            if (infoType == 1) // 1 are commands for settings model
            {
                // in case the command is getConfig command
                if (e.InfoId == (int)InfoEnums.AppConfigInfo)
                {
                    this.InfoUpdate(e);
                    // in case the command is closeHandler command
                }
                else if (e.InfoId == (int)InfoEnums.CloseHandlerInfo)
                {
                    this.RemoveFromHandlersList(e);
                }
            }
        }

        /// <summary>
        /// A generic send to server function. we send a command and an item.
        /// </summary>
        /// <param name="commandEnum">The type of command we send</param>
        /// <param name="item">The path of the handler. If we don't pick any specific handler this will be "Empty" string.</param>
        public void SendCommandToServer(CommandEnum commandEnum, string item)
        {
            string[] args = { };
            // we remove a specific handler
            // if item is not an empty string we initialize args[0] as item, and send it as args.
            if (!item.Equals(""))
            {
                args = new string[1];
                args[0] = item;
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)commandEnum, args, item);
                SendInfo?.Invoke(this, e);
            }
            else // we receive info from the server
            {
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)commandEnum, args, "Empty");
                SendInfo?.Invoke(this, e);
            }
        }

        /// <summary>
        /// The function updates the values of the fields
        /// </summary>
        public void InfoUpdate(InfoEventArgs e)
        {
            // get the information into an array of strings.
            string[] answer = e.Args;
            // set the fields as the received values.
            this.OutputDir = answer[0];
            this.SourceName = answer[1];
            this.LogName = answer[2];
            this.ThumbnailSize = answer[3];
            for (int i = 4; i < answer.Length; i++)
            {
                this.AddToHandlersList(answer[i]);
            }
            PhotosEventArgs args = new PhotosEventArgs(OutputDir);
            sendPath?.Invoke(this, args);
            // set flag of info received
            this.InfoReceived = true;
        }
    }
}
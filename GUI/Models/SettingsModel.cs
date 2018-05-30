using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Infrastructure.Modal.Event;
using GUI.VMs;
using GUI.Connection;
using Infrastructure.Enums;

namespace GUI.Models
{

    /// <summary>
    /// A model for the settings window.
    /// handles the window's logic.
    /// </summary>
    class SettingsModel : INotifyPropertyChanged, ConnectionInterface
    {
        // an event that raises when a property is being changed
        public event PropertyChangedEventHandler PropertyChanged;

        private Communication m_Connection;

        public event EventHandler<CommandRecievedEventArgs> SendInfo;

        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // the output directory 
        private string m_OutputDirectory;
        public string OutputDir {
            get { return m_OutputDirectory; }
            set
            {
                m_OutputDirectory = "Output Directory:  " + value;
                OnPropertyChanged("OutputDirectory");
            }
        }

        // the name of the source
        private string m_SourceName;
        public string SourceName
        {
            get { return m_SourceName; }
            set
            {
                m_SourceName = "Source Name:    " + value;
                OnPropertyChanged("SourceName");
            }
        }

        // the name of the log
        private string m_LogName;
        public string LogName
        {
            get { return m_LogName; }
            set
            {
                m_LogName = "Log Name:  " + value;
                OnPropertyChanged("LogName");
            }
        }

        // the size of the thumbnail
        private string m_ThumbSize;
        public string ThumbSize
        {
            get { return m_ThumbSize; }
            set
            {
                m_ThumbSize = "Thumbnail Size:  " + value;
                OnPropertyChanged("ThumbSize");
            }
        }

        // the selected handler to remove
        private string m_SelectedHandler;
        public string SelectedHandler
        {
            get { return m_SelectedHandler; }
            set {
                m_SelectedHandler = value;
                OnPropertyChanged("SelectedHandler");
            }
        }

        // the list of the directories we listen to.
        private ObservableCollection<string> m_Directories;
        public ObservableCollection<string> Directories
        {
            get { return m_Directories; }
            set
            {
                m_Directories = value;
                OnPropertyChanged("Directories");
            }
        }


        /// <summary>
        /// The function adds a directory to the handlers list
        /// </summary>
        /// <param name="s">The path of the directory</param>
        public void AddToHandlersList(string s)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { m_Directories.Add(s); }));
        }


        /// <summary>
        /// The function removes a directory from the list of directories we listen to.
        /// </summary>
        /// <param name="e">The path of the directory we want to remove</param>
        public void RemoveFromHandlersList(InfoEventArgs e)
        {
            string[] args = e.Args;
            // get the path of the directory
            string DirToRemove = args[0];
            Application.Current.Dispatcher.Invoke(new Action(() => { m_Directories.Remove(DirToRemove); }));
            OnPropertyChanged("Directories");
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsModel() 
        {
            // initialize default values.
            m_OutputDirectory = "Output Directory:  ";
            m_SourceName = "Source Name:    ";
            m_LogName = "Log Name:  ";
            m_ThumbSize = "Thumbnail Size:  ";
            m_Directories = new ObservableCollection<string>();
            Object locker = new object();
            BindingOperations.EnableCollectionSynchronization(m_Directories, locker);
            m_Connection = Communication.CreateConnectionChannel();
            // sign to the event of receive info from the server
            m_Connection.InfoRecieved += GetInfoFromServer;
            SendInfo += m_Connection.StartSenderChannel;
            System.Threading.Thread.Sleep(50);
            // initialize the fields
            this.SendCommandToServer(CommandEnum.GetConfigCommand, "");
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
            this.ThumbSize = answer[3];
            for (int i = 4; i < answer.Length; i++)
            {
                this.AddToHandlersList(answer[i]);
            }
        }
    }
}

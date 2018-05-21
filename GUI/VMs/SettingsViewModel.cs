using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GUI.Models;
using GUI.Connection;
using Prism.Commands;
using Infrastructure.Enums;
using Infrastructure.Modal.Event;

namespace GUI.VMs
{
    /// <summary>
    /// A View-Model for the settings window
    /// </summary>
    class SettingsViewModel : INotifyPropertyChanged, ConnectionInterface
    {
        // members: a model for settings and a model for the communication with the server
        private SettingsModel SettingsModel;
        private Communication m_Connection;

        // The path of the output directory
        public string VM_OutputDirectory
        {
            get { return this.SettingsModel.OutputDir; }
            set
            {
                this.SettingsModel.OutputDir = value;
            }
        }

        // The name of the source
        public string VM_SourceName {
            get { return this.SettingsModel.SourceName; }
            set
            {
                this.SettingsModel.SourceName = value;
            }
        }

        // The name of the logger
        public string VM_LogName {
            get { return this.SettingsModel.LogName; }
            set
            {
                this.SettingsModel.LogName = value;
            }
        }

        // The size of the Thumbnail images.
        public string VM_ThumbSize {
            get { return this.SettingsModel.ThumbSize; }
            set
            {
                this.SettingsModel.ThumbSize = value;
            }
        }
        
        // The selected handler in the list of hanlders.
        public string VM_SelectedHandler
        {
            get { return this.SettingsModel.SelectedHandler; }
            set
            {
                this.SettingsModel.SelectedHandler = value;
                var command = this.RemoveCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
        }

        // The list of handlers.
        public ObservableCollection<string> VM_Directories
        {
            get { return this.SettingsModel.Directories; }
            set
            {
                this.SettingsModel.Directories = value;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public event EventHandler<CommandRecievedEventArgs> SendInfo;

        /// <summary>
        /// constructor
        /// </summary>
        public SettingsViewModel()
        {
            SettingsModel = new SettingsModel();
            // sign to the event of property changed
            SettingsModel.PropertyChanged +=
                 delegate (Object sender, PropertyChangedEventArgs e) {
                 NotifyPropertyChanged("VM_" + e.PropertyName);
               };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);

            m_Connection = Communication.CreateConnectionChannel();
            // sign to the event of receive info from the server
            m_Connection.InfoRecieved += GetInfoFromServer;
            SendInfo += m_Connection.StartSenderChannel;
            System.Threading.Thread.Sleep(50);
            // initialize the fields
            this.SendCommandToServer(CommandEnum.GetConfigCommand, "");
        }

        public ICommand RemoveCommand { get; private set; }

        /// <summary>
        /// When pressing the remove button
        /// </summary>
        private void OnRemove(object obj)
        {
            this.SendToServer();
        }

        /// <summary>
        /// Determines if we can press the remove button or not
        /// </summary>
        /// <returns>false if the selected handler is null or empty, otherwise returns true</returns>
        private bool CanRemove(object obj)
        {
            if (string.IsNullOrEmpty(this.SettingsModel.SelectedHandler))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Notify all the binded elements that the values have changed.
        /// </summary>
        /// <param name="name">The name of the field that have changed</param>
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// A generic send to server function. we send a command and an item.
        /// </summary>
        /// <param name="commandEnum">The type of command we send</param>
        /// <param name="item">The path of the handler. If we don't pick any specific handler this will be empty string.</param>
        public void SendCommandToServer(CommandEnum commandEnum, string item)
        {
            string[] args = { };
            // we remove a specific handler
            // if item is not an empty string we initialize args[0] as item, and send it as args.
            if (!item.Equals("")) {
                args = new string[1];
                args[0] = item;
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)commandEnum, args, item);
                SendInfo?.Invoke(this, e);
            } else // we receive info from the server
            {
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)commandEnum, args, "Empty");
                SendInfo?.Invoke(this, e);
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
                if(e.InfoId == (int)InfoEnums.AppConfigInfo)
                {
                    this.SettingsModel.InfoUpdate(e);
                // in case the command is closeHandler command
                } else if(e.InfoId == (int)InfoEnums.CloseHandlerInfo)
                {
                    this.SettingsModel.RemoveFromHandlersList(e);
                }
            }
        }

        /*** OPTIONAL
        public void window1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.VM_SelectedHandler = null;
        }*/

        /// <summary>
        /// Send a close handler command to the server
        /// </summary>
        public void SendToServer()
        {
            SendCommandToServer(CommandEnum.CloseCommand, this.VM_SelectedHandler);
        }
    }
}

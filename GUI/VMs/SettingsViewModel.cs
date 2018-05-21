﻿using System;
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
    class SettingsViewModel : INotifyPropertyChanged, ConnectionInterface
    {
        private SettingsModel SettingsModel;
        private Communication m_Connection;

        public string VM_OutputDirectory
        {
            get { return this.SettingsModel.OutputDir; }
            set
            {
                this.SettingsModel.OutputDir = value;
            }
        }

        public string VM_SourceName {
            get { return this.SettingsModel.SourceName; }
            set
            {
                this.SettingsModel.SourceName = value;
            }
        }

        public string VM_LogName {
            get { return this.SettingsModel.LogName; }
            set
            {
                this.SettingsModel.LogName = value;
            }
        }

        public string VM_ThumbSize {
            get { return this.SettingsModel.ThumbSize; }
            set
            {
                this.SettingsModel.ThumbSize = value;
            }
        }
        
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

        public ObservableCollection<string> VM_Directories
        {
            get { return this.SettingsModel.Directories; }
            set
            {
                this.SettingsModel.Directories = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public event EventHandler<CommandRecievedEventArgs> sendInfo;

        public SettingsViewModel()
        {
            SettingsModel = new SettingsModel();
            SettingsModel.PropertyChanged +=
                 delegate (Object sender, PropertyChangedEventArgs e) {
                 NotifyPropertyChanged("VM_" + e.PropertyName);
               };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);

            m_Connection = Communication.CreateConnectionChannel();
            // getting the initialize info from the server
            m_Connection.InfoRecieved += getInfoFromServer;
            sendInfo += m_Connection.StartSenderChannel;
            System.Threading.Thread.Sleep(50);
            // initialize the fields
            this.SendCommandToServer(CommandEnum.GetConfigCommand, "");
        }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove(object obj)
        {
            this.sendToServer();
        }

        private bool CanRemove(object obj)
        {
            if (string.IsNullOrEmpty(this.SettingsModel.SelectedHandler))
            {
                return false;
            }
            return true;
        }


        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void SendCommandToServer(CommandEnum commandEnum, string item)
        {
            string[] args = { };
            if (!item.Equals("")) {
                args = new string[1];
                args[0] = item;
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)commandEnum, args, item);
                sendInfo?.Invoke(this, e);
            } else // we remove a specific handler
            {
                CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)commandEnum, args, "Empty");
                sendInfo?.Invoke(this, e);
            }
        }

        public void getInfoFromServer(object sender, InfoEventArgs e)
        {
            int infoType = InfoReceivedParser.parseInfoType(e.InfoId);
            if (infoType == 1) // 1 are commands for settings model
            {
                if(e.InfoId == (int)InfoEnums.AppConfigInfo)
                {
                    this.SettingsModel.InfoUpdate(e);
                } else if(e.InfoId == (int)InfoEnums.CloseHandlerInfo)
                {
                    this.SettingsModel.RemoveFromHandlersList(e);
                }
            }
        }

        public void sendToServer()
        {
            SendCommandToServer(CommandEnum.CloseCommand, this.VM_SelectedHandler);
        }
    }
}

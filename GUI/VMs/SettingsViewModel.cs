using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GUI.Models;
using Prism.Commands;
using GUI.Modal.Event;
using GUI.Enums;

namespace GUI.VMs
{
    class SettingsViewModel : INotifyPropertyChanged//, ConnectionInterface
    {
        private SettingsModel SettingsModel;
        private Model m_ConnectionModel;

        public string OutputDirectory
        {
            get { return this.SettingsModel.OutputDir; }
            set
            {
                this.SettingsModel.OutputDir = value;
            }
        }

        public string SourceName {
            get { return this.SettingsModel.SourceName; }
            set
            {
                this.SettingsModel.SourceName = value;
            }
        }

        public string LogName {
            get { return this.SettingsModel.LogName; }
            set
            {
                this.SettingsModel.LogName = value;
            }
        }

        public string ThumbSize {
            get { return this.SettingsModel.ThumbSize; }
            set
            {
                this.SettingsModel.ThumbSize = value;
            }
        }
        
        public string SelectedHandler
        {
            get { return this.SettingsModel.SelectedHandler; }
            set
            {
                this.SettingsModel.SelectedHandler = value;
                var command = this.RemoveCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<string> Directories
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

            m_ConnectionModel = Model.CreateConnectionChannel();
            // getting the initialize info from the server
            m_ConnectionModel.InfoRecieved += getInfoFromServer;
            sendInfo += m_ConnectionModel.StartSenderChannel;
            m_ConnectionModel.start();
            // initialize the fields
            this.ReuqestAppConfigFromServer();
        }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove(object obj)
        {
            //this.sendToServer();
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

        public void ReuqestAppConfigFromServer()
        {
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, args, "Empty");
            sendInfo?.Invoke(this, e);
        }

        public void getInfoFromServer(object sender, InfoEventArgs e)
        {
            int infoType = InfoReceivedParser.parseInfoType(e.InfoId);
            if (infoType == 1)
            {
                if(e.InfoId == (int)InfoEnums.AppConfigInfo)
                {
                    infoUpdate(e);
                }
            }
        }

        private void infoUpdate(InfoEventArgs e)
        {
            string[] answer = e.Args;
            SettingsModel.OutputDir = answer[0];
            SettingsModel.SourceName = answer[1];
            SettingsModel.LogName = answer[2];
            SettingsModel.ThumbSize = answer[3];
            for (int i = 4; i < answer.Length; i++)
            {
                SettingsModel.addToHandlersList(answer[i]);
            }
        }
    }
}

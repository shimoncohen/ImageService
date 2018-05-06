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

namespace GUI.VMs
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsModel SettingsModel;

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

        public SettingsViewModel()
        {
            SettingsModel = new SettingsModel();
            SettingsModel.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e) {
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove); 
        }

        public ICommand RemoveCommand { get; private set; }

        private void OnRemove(object obj)
        {
            this.SettingsModel.sendToServer();
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
    }
}

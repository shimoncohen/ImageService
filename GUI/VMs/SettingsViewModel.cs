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
    class SettingsViewModel : INotifyPropertyChanged
    {
        // members: a model for settings
        private SettingsModel SettingsModel;

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
        }

        public ICommand RemoveCommand { get; private set; }

        /// <summary>
        /// When pressing the remove button
        /// </summary>
        private void OnRemove(object obj)
        {
            this.SettingsModel.SendCommandToServer(CommandEnum.CloseCommand, this.VM_SelectedHandler);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace GUI.Models
{
    class SettingsModel : INotifyPropertyChanged
    {
        // an event that raises when a property is being changed
        public event PropertyChangedEventHandler PropertyChanged;
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
                m_OutputDirectory = "Output Directory:" + value;
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
                m_OutputDirectory = "Source Name:" + value;
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
                m_LogName = "Log Name:" + value;
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
                m_OutputDirectory = "Thumbnail Size:" + value;
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

        public SettingsModel() 
        {
            m_OutputDirectory = "Output Directory:";
            m_SourceName = "Source Name:";
            m_LogName = "Log Name:";
            m_ThumbSize = "Thumbnail Size:";
            m_Directories = new ObservableCollection<string>();
            m_Directories.Add("Test!!!!!");
        }
    }
}

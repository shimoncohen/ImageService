﻿using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Infrastructure.Modal.Event;

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
        
        public void AddToHandlersList(string s)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { m_Directories.Add(s); }));
        }

        public void RemoveFromHandlersList(InfoEventArgs e)
        {
            string[] args = e.Args;
            string DirToRemove = args[0];
            Application.Current.Dispatcher.Invoke(new Action(() => { m_Directories.Remove(DirToRemove); }));
            OnPropertyChanged("Directories");
        }

        public SettingsModel() 
        {
            m_OutputDirectory = "Output Directory:  ";
            m_SourceName = "Source Name:    ";
            m_LogName = "Log Name:  ";
            m_ThumbSize = "Thumbnail Size:  ";
            m_Directories = new ObservableCollection<string>();
            Object locker = new object();
            BindingOperations.EnableCollectionSynchronization(m_Directories, locker);
        }

        public void InfoUpdate(InfoEventArgs e)
        {
            string[] answer = e.Args;
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

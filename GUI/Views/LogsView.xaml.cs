﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GUI.VMs;

namespace GUI.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : UserControl
    {
        private LogsViewModel LogsViewModel;

        public LogsView()
        {
            InitializeComponent();
            LogsViewModel = new LogsViewModel();
            this.DataContext = LogsViewModel;
        }
    }
}

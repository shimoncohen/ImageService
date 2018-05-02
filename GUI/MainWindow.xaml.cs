using System;
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
using GUI;
using GUI.VMs;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsViewModel SettingsViewModel;
        private LogsViewModel LogsViewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.SettingsViewModel = new SettingsViewModel();
            this.LogsViewModel = new LogsViewModel();
            this.DataContext = SettingsViewModel;
            this.DataContext = LogsViewModel;
        }
    }
}

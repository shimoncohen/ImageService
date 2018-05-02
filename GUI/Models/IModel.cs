using Infrastructure.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
    interface IModel
    {
        // need to hold appConfig info
        // needs to hols logs
        event EventHandler<InfoEventArgs> InfoRecieved;
        string AppConfig { get; }
        string Logs { get; }

        void start();
        void StartSenderChannel(object sender, CommandRecievedEventArgs e);
        void StartRecieverChannel();
        void stop();
    }
}

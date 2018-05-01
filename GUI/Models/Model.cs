using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Infrastructure.Modal.Event;
using Infrastructure.Enums;

namespace GUI.Models
{
    class Model : IModel
    {
        public event EventHandler<InfoEventArgs> InfoRecieved;
        private const int serverPort = 8000;
        private string appConfig;
        private string logs;
        private Socket socket;

        public string AppConfig {
            get { return appConfig; }
            set { }
        }

        public string Logs
        {
            get { return logs; }
            set { }
        }

        public Model()
        {

        }

        public void start()
        {
            IPHostEntry iphostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAdress = iphostInfo.AddressList[0];
            IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, 8080);

            socket = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            new Task(() =>
            {
                StartRecieverChannel();
            }).Start();
        }

        public void StartSenderChannel(object sender, CommandRecievedEventArgs e)
        {

        }

        public void StartRecieverChannel()
        {
            new Task(() =>
            {
                //TODO: need to read stream of info from server and send arguments to the event
                string[] args = { };
                InfoEventArgs e = new InfoEventArgs((int)InfoEnums.CloseHandlerInfo, args);
                InfoRecieved?.Invoke(this, e);
            }).Start();
        }

        public void StartSenderChannel()
        {
            throw new NotImplementedException();
        }

        public void stop()
        {
            socket.Close();
        }
    }
}

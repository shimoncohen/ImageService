using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Infrastructure.Modal.Event;
using Infrastructure.Enums;
using System.IO;
using Newtonsoft.Json;

namespace GUI.Models
{
    class Model : IModel
    {
        public event EventHandler<InfoEventArgs> InfoRecieved;
        private const int serverPort = 8000;
        private string appConfig;
        private string logs;
        private TcpClient client;

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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
            client = new TcpClient();
            client.Connect(ep);
            StartRecieverChannel();
        }

        public void StartSenderChannel(object sender, CommandRecievedEventArgs e)
        {
            new Task(() =>
            {
                while (client.Connected)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string args = JsonConvert.SerializeObject(e);
                        writer.Write(args);
                    }
                }
            }).Start();
        }

        public void StartRecieverChannel()
        {
            new Task(() =>
            {
                while (client.Connected)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string args = reader.ReadLine();
                        InfoEventArgs e = JsonConvert.DeserializeObject<InfoEventArgs>(args);
                        InfoRecieved?.Invoke(this, e);
                    }
                }
            }).Start();
        }

        public void stop()
        {
            client.Close();
        }
    }
}

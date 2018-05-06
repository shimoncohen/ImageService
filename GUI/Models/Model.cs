using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using GUI.Modal.Event;

namespace GUI.Models
{
    public class Model : IModel
    {
        private static Model model;
        private const int serverPort = 8000;
        private string appConfig;
        private string logs;
        private TcpClient client;

        public event EventHandler<InfoEventArgs> InfoRecieved;

        public string AppConfig {
            get { return appConfig; }
            set { }
        }

        public string Logs
        {
            get { return logs; }
            set { }
        }

        private Model()
        {
            
        }

        public static Model CreateConnectionChannel()
        {
            // if not already created
            if (model == null)
            {
                model = new Model();
            }
            // otherwise create new instance
            return model;
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

        private void StartRecieverChannel()
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

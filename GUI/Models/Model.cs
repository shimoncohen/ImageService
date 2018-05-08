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
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;

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
                stream = client.GetStream();
                writer = new BinaryWriter(stream);
                string args = JsonConvert.SerializeObject(e);
                writer.Write(args);
            }).Start();
        }

        private void StartRecieverChannel()
        {
            new Task(() =>
            {
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                while (client.Connected)
                {
                    string args = reader.ReadString();
                    InfoEventArgs e = JsonConvert.DeserializeObject<InfoEventArgs>(args);
                    InfoRecieved?.Invoke(this, e);
                }
            }).Start();
        }

        public void stop()
        {
            client.Close();
        }
    }
}

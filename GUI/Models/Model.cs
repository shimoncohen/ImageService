using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using GUI.Modal.Event;
using System.Threading;
using GUI.Enums;

namespace GUI.Models
{
    public class Model : IModel
    {
        private static Model model;
        private const int serverPort = 8000;
        private Mutex mutex;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;
        private Boolean active = false;

        public event EventHandler<InfoEventArgs> InfoRecieved;

        private Model()
        {
            mutex = new Mutex();
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
            if(client == null)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
                client = new TcpClient();
                try
                {
                    client.Connect(ep);
                } catch(Exception e)
                {
                    return;
                }
            }
        }

        public void StartSenderChannel(object sender, CommandRecievedEventArgs e)
        {
            new Task(() =>
            {
                if (client.Connected)
                {
                    stream = client.GetStream();
                    //mutex.WaitOne();
                    writer = new BinaryWriter(stream);
                    //mutex.ReleaseMutex();
                    string args = JsonConvert.SerializeObject(e);
                    mutex.WaitOne();
                    writer.Write(args);
                    mutex.ReleaseMutex();
                }
            }).Start();
        }

        public void StartRecieverChannel ()
        {
            if(!active)
            {
                string args;
                active = true;
                new Task(() =>
                {
                    if (client.Connected)
                    {
                        stream = client.GetStream();
                        mutex.WaitOne();
                        reader = new BinaryReader(stream);
                        mutex.ReleaseMutex();
                    }
                    while (client.Connected)
                    {
                        try
                        {
                            args = reader.ReadString();
                        }
                        catch (Exception error)
                        {
                            return;
                        }
                        InfoEventArgs e = JsonConvert.DeserializeObject<InfoEventArgs>(args);
                        InfoRecieved?.Invoke(this, e);
                    }
                }).Start();
            }
        }

        public void stop()
        {
            client.Close();
        }

        public bool IsConnected()
        {
            return client.Connected;
        }
    }
}

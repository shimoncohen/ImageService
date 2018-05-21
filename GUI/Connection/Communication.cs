using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using Infrastructure.Modal.Event;
using System.Threading;
using System.Diagnostics;

namespace GUI.Connection
{
    public class Communication : ICommunication
    {
        private static Communication model;
        // the port number
        private const int serverPort = 8000;
        // a mutex
        private Mutex mutex;
        // a tcp client
        private TcpClient client;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;

        public event EventHandler<InfoEventArgs> InfoRecieved;

        /// <summary>
        /// constructor
        /// </summary>
        private Communication()
        {
            mutex = new Mutex();
            Start();
            StartRecieverChannel();
        }

        /// <summary>
        /// Create the connection as a singelton
        /// </summary>
        public static Communication CreateConnectionChannel()
        {
            // if not already created
            if (model == null)
            {
                model = new Communication();
            }
            // otherwise create new instance
            return model;
        }

        /// <summary>
        /// start the communication
        /// </summary>
        public void Start()
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

        /// <summary>
        /// The function sends a request to the server
        /// </summary>
        /// <param name= sender> the class that invoked the event </param>
        /// <param name= e> the command arguments to send to the server </param>
        public void StartSenderChannel(object sender, CommandRecievedEventArgs e)
        {
            new Task(() =>
            {
                if (client.Connected)
                {
                    // open stream and writer
                    stream = client.GetStream();
                    writer = new BinaryWriter(stream);
                    string args = JsonConvert.SerializeObject(e);
                    try
                    {
                        // write to the server
                        mutex.WaitOne();
                        writer.Write(args);
                        mutex.ReleaseMutex();
                    } catch (Exception e1)
                    {
                        mutex.ReleaseMutex();
                        Debug.WriteLine("In GUI communication, failed send, Error: " + e1.ToString());
                        return;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Read information to the server with a streamer.
        /// </summary>
        private void StartRecieverChannel()
        {
            string args;
            new Task(() =>
            {
                if (client.Connected)
                {
                    // open a stream and a reader.
                    stream = client.GetStream();
                    reader = new BinaryReader(stream);
                }
                while (client.Connected)
                {
                    try
                    {
                        // read from the server.
                        args = reader.ReadString();
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine("In GUI communication, failed read, Error: " + error.ToString());
                        break;
                    }
                    // deserialize the info from the server into an InfoEventArgs.
                    InfoEventArgs e = JsonConvert.DeserializeObject<InfoEventArgs>(args);
                    // invoke the info received event
                    InfoRecieved?.Invoke(this, e);
                }
            }).Start();
        }

        /// <summary>
        /// stops the communication
        /// </summary>
        public void Stop()
        {
            client.Close();
        }

        /// <summary>
        /// The function returns the true if the tcp client is connected and false if it isn't
        /// </summary>
        public bool IsConnected()
        {
            return client.Connected;
        }
    }
}

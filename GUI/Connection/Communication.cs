using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using GUI.Modal.Event;
using System.Threading;
using System.Diagnostics;

namespace GUI.Connection
{
    public class Communication : ICommunication
    {
        private static Communication model;
        private const int serverPort = 8000;
        private Mutex mutex;
        private TcpClient client;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;

        public event EventHandler<InfoEventArgs> InfoRecieved;

        private Communication()
        {
            mutex = new Mutex();
            StartRecieverChannel();
        }

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
                    writer = new BinaryWriter(stream);
                    string args = JsonConvert.SerializeObject(e);
                    try
                    {
                        mutex.WaitOne();
                        if(client.Connected)
                        {
                            writer.Write(args);
                        }
                        mutex.ReleaseMutex();
                    } catch (Exception e1)
                    {
                        Debug.WriteLine("In GUI communication, failed send, Error: " + e1.ToString());
                        return;
                    }
                }
            }).Start();
        }

        private void StartRecieverChannel()
        {
            string args;
            new Task(() =>
            {
                if (client.Connected)
                {
                    stream = client.GetStream();
                    reader = new BinaryReader(stream);
                }
                while (client.Connected)
                {
                    try
                    {
                        //mutex.WaitOne();
                        if (client.Connected)
                        {
                            args = reader.ReadString();
                        } else
                        {
                            //mutex.ReleaseMutex();
                            break;
                        }
                        //mutex.ReleaseMutex();
                    }
                    catch (Exception error)
                    {
                        Debug.WriteLine("In GUI communication, failed read, Error: " + error.ToString());
                        break;
                    }
                    InfoEventArgs e = JsonConvert.DeserializeObject<InfoEventArgs>(args);
                    InfoRecieved?.Invoke(this, e);
                }
                //CloseResources(stream, reader, writer);
            }).Start();
        }

        /*private void CloseResources(Stream stream, BinaryReader reader = null, BinaryWriter writer = null)
        {
            stream.Dispose();
            if(reader != null)
            {
                reader.Close();
            }
            if (writer != null)
            {
                writer.Close();
            }
        }*/

        public void stop()
        {
            //CloseResources(stream, reader, writer);
            client.Close();
        }

        public bool IsConnected()
        {
            return client.Connected;
        }
    }
}

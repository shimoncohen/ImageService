using ImageServiceWeb.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ImageServiceWebModel
    {
        private string status;
        private int numOfPics;
        public ImageServiceWebModel(PhotoList photoList)
        {
            Communication communication = Communication.CreateConnectionChannel();
            bool connected = communication.IsConnected();
            numOfPics = photoList.Length();
            status = ConnectionStatus(connected);
        }

        private string ConnectionStatus(bool connected)
        {
            switch(connected)
            {
                case true:
                    return "connected";
                default:
                    return "disconnected";
            }
        }
    }
}
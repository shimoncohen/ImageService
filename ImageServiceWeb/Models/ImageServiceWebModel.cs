using ImageServiceWeb.Connection;
using ImageServiceWeb.WebEventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ImageServiceWebModel
    {
        private string status;
        private int numOfPics;
        private List <Student> info = new List<Student>();

        public ImageServiceWebModel(PhotoListModel photoList)
        {
            Communication communication = Communication.CreateConnectionChannel();
            bool connected = communication.IsConnected();
            numOfPics = photoList.Length();
            status = ConnectionStatus(connected);
            this.ParseInfo();
        }

        public string GetStatus { get { return this.status; } }
        public int GetNumofPics { get { return this.numOfPics; }
            set
            {
                this.numOfPics = value;
            }
        }
        public List<Student> GetInfo { get { return this.info; } }

        private string ConnectionStatus(bool connected)
        {
            switch(connected)
            {
                case true:
                    return "Connected";
                default:
                    return "Disconnected";
            }
        }

        private void ParseInfo()
        {
            string text;
            string[] line;
            StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/details.txt"));
            text = file.ReadLine();
            while(text != null)
            {
                line = text.Split(' ');
                this.info.Add(new Student(line[0], line[1], line[2]));
                text = file.ReadLine();
            }
            file.Close();
        }
        
        public void UpdatePhotosNum(object sender, PhotoCountEventArgs photoCountEventArgs)
        {
            int temp = (int)photoCountEventArgs.Count;
            this.GetNumofPics = temp;
        }
    }


    public class Student
    {
        private string firstName, lastName, ID;
        public Student(string first, string last, string id)
        {
            this.firstName = first;
            this.lastName = last;
            this.ID = id;
        }

        public string GetFirstName {
            get { return this.firstName; }
        }
        public string GetLastName {
            get { return this.lastName; }
        }
        public string GetID { get { return this.ID; } }
    }
}
using ImageServiceWeb.Connection;
using ImageServiceWeb.WebEventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// The class for the ImageServiceWeb model
    /// </summary>
    public class ImageServiceWebModel
    {
        // members
        private string status;
        private int numOfPics;
        private List <Student> info = new List<Student>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photoList">The photo list</param>
        public ImageServiceWebModel(PhotoListModel photoList)
        {
            // connect to the server
            Communication communication = Communication.CreateConnectionChannel();
            // get the connection status of the server
            bool connected = communication.IsConnected();
            // get the number of pics
            numOfPics = photoList.Length();
            status = ConnectionStatus(connected);
            // parse the students info from file
            this.ParseInfo();
        }

        /// <summary>
        /// getter for the connection status
        /// </summary>
        public string GetStatus { get { return this.status; } }

        /// <summary>
        /// getter and setter for the number of photos
        /// </summary>
        public int GetNumofPics { get { return this.numOfPics; }
            set
            {
                this.numOfPics = value;
            }
        }

        /// <summary>
        /// getter for the students info
        /// </summary>
        public List<Student> GetInfo { get { return this.info; } }

        /// <summary>
        /// The function returns a string that indicates the connection status of the server
        /// </summary>
        /// <param name="connected">The connection status as a boolean</param>
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

        /// <summary>
        /// The function reads the info of the students from a file
        /// </summary>
        private void ParseInfo()
        {
            string text;
            string[] line;
            StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/details.txt"));
            text = file.ReadLine();
            while(text != null)
            {
                // split to first name, last name, id
                line = text.Split(' ');
                this.info.Add(new Student(line[0], line[1], line[2]));
                text = file.ReadLine();
            }
            file.Close();
        }

        /// <summary>
        /// The function updates the number of photos
        /// </summary>
        /// <param name="photoCountEventArgs">The arguments received from the server</param>
        /// <param name="sender">The sender object</param>
        public void UpdatePhotosNum(object sender, PhotoCountEventArgs photoCountEventArgs)
        {
            int temp = (int)photoCountEventArgs.Count;
            this.GetNumofPics = temp;
        }
    }

    /// <summary>
    /// A class that holds the info of the students
    /// </summary>
    public class Student
    {
        private string firstName, lastName, ID;

        /// <summary>
        /// Constructor
        /// </summary>
        public Student(string first, string last, string id)
        {
            this.firstName = first;
            this.lastName = last;
            this.ID = id;
        }

        /// <summary>
        /// getter for the first name
        /// </summary>
        public string GetFirstName {
            get { return this.firstName; }
        }

        /// <summary>
        /// getter for the last name
        /// </summary>
        public string GetLastName {
            get { return this.lastName; }
        }

        /// <summary>
        /// getter for the id
        /// </summary>
        public string GetID { get { return this.ID; } }
    }
}
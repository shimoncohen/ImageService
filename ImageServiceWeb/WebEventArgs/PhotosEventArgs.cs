using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.WebEventArgs
{
    public class PhotosEventArgs : EventArgs
    {
        /// <summary>
        /// getter and setter to the path to the output directory
        /// </summary>
        /// <return> returns the path to the output directory </return>
        public string Path { get; set; }
        //constructor
        public PhotosEventArgs(string path)
        {
            Path = path;
        }
    }
}
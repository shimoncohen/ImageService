using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// The class of handler model
    /// </summary>
    public class HandlerModel
    {
        /// <summary>
        /// getter and setter for the path of the handler
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HandlerModel(string path)
        {
            Path = path;
        }
    }
}
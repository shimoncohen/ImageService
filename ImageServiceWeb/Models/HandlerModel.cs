using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class HandlerModel
    {
        public string Path { get; set; }

        public HandlerModel(string path)
        {
            Path = path;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.EventArgs
{
    public class PhotoCountEventArgs
    {
        /// <summary>
        /// getter and setter to the number of photos
        /// </summary>
        /// <return> returns the number of photos </return>
        public int Count { get; set; }
        //constructor
        public PhotoCountEventArgs(int num)
        {
            Count = num;
        }
    }
}
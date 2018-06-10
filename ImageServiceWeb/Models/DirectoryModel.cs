using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// The model class of a directory
    /// </summary>
    public class DirectoryModel
    {
        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="path">the path of the directory</param>
        public DirectoryModel(string path)
        {
            DirPath = path;
        }

        /// <summary>
        /// getter and setter to the path of the directory
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "DirPath")]
        public string DirPath { get; set; }
    }
}
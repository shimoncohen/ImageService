﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    public class Directory
    {
        public Directory(string path)
        {
            DirPath = path;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "DirPath")]
        public string DirPath { get; set; }
    }
}
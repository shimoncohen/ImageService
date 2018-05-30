﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    public class Directory
    {
        string dirPath;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "DirPath")]
        public string DirPath { get; set; }
    }
}
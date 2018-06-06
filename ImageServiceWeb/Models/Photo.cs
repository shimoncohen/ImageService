using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageServiceWeb.Models
{
    public class Photo
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string PhotoPath { get; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string ThumbPhotoPath { get; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoName")]
        public string PhotoName { get; }

        public Photo(string path, string thumbPath)
        {
            string root = Path.GetPathRoot(path);
            Directory.SetCurrentDirectory(root);
            PhotoPath = path;
            ThumbPhotoPath = thumbPath;
            PhotoName = Path.GetFileName(path);
            Month = Path.GetDirectoryName(path);
            Month = new DirectoryInfo(Month).Name;
            Year = Path.GetDirectoryName(Path.GetDirectoryName(path));
            Year = new DirectoryInfo(Year).Name;
        }
    }
}
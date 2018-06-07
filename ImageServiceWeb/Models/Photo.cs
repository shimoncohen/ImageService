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
        private string directoryPath;

        private string tempPath;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string PhotoPath
        {
            get
            {
                if (tempPath != null)
                {
                    bool current = false, once = false; ;
                    string[] pathParts = tempPath.Contains('/') ? tempPath.Split('/') : tempPath.Split('\\');
                    List<string> gather = new List<string>();
                    foreach (string part in pathParts)
                    {
                        if (!current && !part.Equals("ImageServiceWeb"))
                        {
                            continue;
                        }
                        else if (!once && part.Equals("ImageServiceWeb"))
                        {
                            //gather.Add("~");
                            once = true;
                            current = true;
                        }
                        else
                        {
                            gather.Add(part);
                        }
                    }
                    string[] newPath = gather.ToArray();
                    string endPath = String.Join("/", newPath);
                    return endPath;
                }
                return tempPath;
            }
        }

        private string tempThumbPath;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string ThumbPhotoPath
        {
            get
            {
                if (tempThumbPath != null)
                {
                    bool current = false, once = false; ;
                    string[] pathParts = tempThumbPath.Contains('/') ? tempThumbPath.Split('/') : tempThumbPath.Split('\\');
                    List<string> gather = new List<string>();
                    foreach (string part in pathParts)
                    {
                        if (!current && !part.Equals("ImageServiceWeb"))
                        {
                            continue;
                        }
                        else if (!once && part.Equals("ImageServiceWeb"))
                        {
                            //gather.Add("~");
                            once = true;
                            current = true;
                        }
                        else if(part.Equals("Thumbnails"))
                        {
                            gather.Add("Thumbnails");
                            break;
                        }
                        else
                        {
                            gather.Add(part);
                        }
                    }
                    string[] newPath = gather.ToArray();
                    string endPath = String.Join("/", newPath);
                    return endPath;
                }
                return tempPath;

            }
        }

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

        public Photo(string dirPath, string path, string thumbPath)
        {
            string root = Path.GetPathRoot(path);
            Directory.SetCurrentDirectory(root);
            directoryPath = dirPath;
            tempPath = path;
            tempThumbPath = thumbPath;
            PhotoName = Path.GetFileName(path);
            Month = Path.GetDirectoryName(path);
            Month = new DirectoryInfo(Month).Name;
            Year = Path.GetDirectoryName(Path.GetDirectoryName(path));
            Year = new DirectoryInfo(Year).Name;
        }
    }
}
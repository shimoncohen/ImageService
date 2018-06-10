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
        private string fullPath;
        private string fullThumbPath;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string PhotoPath
        {
            get
            {
                if (fullPath != null)
                {
                    bool current = false, once = false; ;
                    string[] pathParts = fullPath.Contains('/') ? fullPath.Split('/') : fullPath.Split('\\');
                    List<string> gather = new List<string>();
                    List<string> gatherAll = new List<string>();
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
                    string[] newPath;
                    if (once)
                    {
                        newPath = gather.ToArray();
                    }
                    else
                    {
                        newPath = gatherAll.ToArray();
                    }
                    string endPath = String.Join("\\", newPath);
                    return endPath;
                }
                return fullPath;
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string ThumbPhotoPath
        {
            get
            {
                if (fullThumbPath != null)
                {
                    bool current = false, once = false; ;
                    string[] pathParts = fullThumbPath.Contains('/') ? fullThumbPath.Split('/') : fullThumbPath.Split('\\');
                    List<string> gather = new List<string>();
                    List<string> gatherAll = new List<string>();
                    foreach (string part in pathParts)
                    {
                        if (!current && !part.Equals("ImageServiceWeb"))
                        {
                            gatherAll.Add(part);
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
                    string[] newPath;
                    if (once)
                    {
                        newPath = gather.ToArray();
                    }
                    else
                    {
                        newPath = gatherAll.ToArray();
                    }
                    string endPath = String.Join("\\", newPath);
                    return endPath;
                }
                return fullThumbPath;

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
            fullPath = path;
            fullThumbPath = thumbPath;
            PhotoName = Path.GetFileName(path);
            Month = Path.GetDirectoryName(path);
            Month = new DirectoryInfo(Month).Name;
            Year = Path.GetDirectoryName(Path.GetDirectoryName(path));
            Year = new DirectoryInfo(Year).Name;
        }

        public string GetFullPath()
        {
            return this.fullPath;
        }

        public string GetFullThumbPath()
        {
            return this.fullThumbPath;
        }

        public string DirPath()
        {
            return this.directoryPath;
        }
    }
}
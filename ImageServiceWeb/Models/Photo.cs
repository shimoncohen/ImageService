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
    /// <summary>
    /// The class of Photo
    /// </summary>
    public class Photo
    {
        // members
        private string directoryPath;
        private string fullPath;
        private string fullThumbPath;

        /// <summary>
        /// Gets the photo path.
        /// </summary>
        /// <value>
        /// The photo path.
        /// </value>
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
                    // create the path for the photo
                    foreach (string part in pathParts)
                    {
                        if (!current && !part.Equals("ImageServiceWeb"))
                        {
                            continue;
                        }
                        else if (!once && part.Equals("ImageServiceWeb"))
                        {
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

        /// <summary>
        /// Gets the thumb photo path.
        /// </summary>
        /// <value>
        /// The thumb photo path.
        /// </value>
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
                    // create the path to the thumbnail
                    foreach (string part in pathParts)
                    {
                        if (!current && !part.Equals("ImageServiceWeb"))
                        {
                            gatherAll.Add(part);
                            continue;
                        }
                        else if (!once && part.Equals("ImageServiceWeb"))
                        {
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

        /// <summary>
        /// Gets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; }

        /// <summary>
        /// Gets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; }

        /// <summary>
        /// Gets the name of the photo.
        /// </summary>
        /// <value>
        /// The name of the photo.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoName")]
        public string PhotoName { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dirPath">The path to the directory</param>
        /// <param name="path">The path to the photo</param>
        /// <param name="thumbPath">The path to the thumbnail</param>
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

        /// <summary>
        /// Gets the full path to the photo.
        /// </summary>
        /// <returns></returns>
        public string GetFullPath()
        {
            return this.fullPath;
        }

        /// <summary>
        /// Gets the full path to the thumbnail.
        /// </summary>
        /// <returns></returns>
        public string GetFullThumbPath()
        {
            return this.fullThumbPath;
        }

        /// <summary>
        /// returns the path to the directory.
        /// </summary>
        /// <returns>the path to the directory</returns>
        public string DirPath()
        {
            return this.directoryPath;
        }
    }
}
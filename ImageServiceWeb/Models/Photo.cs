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
        [Display(Name = "DirPath")]
        public string PhotoPath { get; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Creation")]
        public DateTime Creation { get; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; }

        public Photo(string path)
        {
            PhotoPath = path;
            //Creation = GetDateFromImage(path);
            Month = Path.GetDirectoryName(path);
            Year = Path.GetDirectoryName(Path.GetDirectoryName(path));
        }

        /*private DateTime GetDateFromImage(string path)
        {
            DateTime timeCreated = new DateTime();
            try
            {
                // extract images creation date and time
                //timeCreated = File.GetCreationTime(path);
                timeCreated = this.GetDateTakenFromImage(path);
            }
            catch (Exception e)
            {
                try
                {
                    timeCreated = this.GetDateCreatedFromImage(path);
                }
                catch (Exception e1)
                {
                    return DateTime.Now;
                }
            }
            return timeCreated;
        }

        //retrieves the datetime WITHOUT loading the whole image
        private DateTime GetDateTakenFromImage(string path)
        {
            Regex r = new Regex(":");
            using (Image myImage = Image.FromFile(path))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                myImage.Dispose();
                return DateTime.Parse(dateTaken);
            }
        }

        // <summary>
        // the function gets the creation date of an image.
        // </summary>
        // <param name= path> the path of the file </param>
        // <return> the creation date of the image as an object of DateTime </return>
        private DateTime GetDateCreatedFromImage(string path)
        {
            DateTime now = DateTime.Now;
            TimeSpan localOffset = now - now.ToUniversalTime();
            return File.GetLastWriteTimeUtc(path) + localOffset;
        }*/
    }
}
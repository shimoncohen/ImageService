using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public ImageServiceModal(string path, int size)
        {
            this.m_OutputFolder = path;
            this.m_thumbnailSize = size;
        }

        /*
         * adds an image to output dir as a thumbnail
         */
        public string AddFile(string path, out bool result)
        {
            if(File.Exists(path)) {
                // create output dir if dosent exist
                Directory.CreateDirectory(this.m_OutputFolder);
                // extract images creation date and time
                DateTime timeCreated = File.GetCreationTime(path);
                int year = timeCreated.Year;
                int month = timeCreated.Month;

                // make new path to wanted folder
                string newPath = this.m_OutputFolder+"\\"+year.ToString()+"\\"+month.ToString();
                Directory.CreateDirectory(newPath);

                // extracting the name of the image and appending it the new path
                newPath = newPath+path.Substring(path.LastIndexOf("\\"));

                // save image as a thumbnail
                Image image = Image.FromFile(path);
                Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                thumb.Save(Path.ChangeExtension(newPath, "thumb"));
                image.Dispose();
                thumb.Dispose();
                result = true;


                // TODO: think if we can send this class the logger to write that a new file was added


                return newPath;
            }
            result = false;
            return "Source file doesn't exist";
        }

        //retrieves the datetime WITHOUT loading the whole image
    }
    #endregion
}

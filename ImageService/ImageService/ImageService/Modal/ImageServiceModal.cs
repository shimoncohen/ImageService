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
        // The Output Folder
        private string m_OutputFolder;
        // The Size Of The Thumbnail Size
        private int m_thumbnailSize;

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
                // create output dir if doesn't exist
                DirectoryInfo dirInfo = Directory.CreateDirectory(this.m_OutputFolder);
                // create as a hidden directory
                dirInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                // extract images creation date and time
                DateTime timeCreated = File.GetCreationTime(path);
                int year = timeCreated.Year;
                int month = timeCreated.Month;

                // make new path to output folder
                string newPath = this.m_OutputFolder+"\\"+year.ToString()+"\\"+month.ToString();
                // make new path to thumbnail folder
                string thumbNewPath = this.m_OutputFolder+"\\Thumbnails"+"\\"+year.ToString()+"\\"+month.ToString();
                // create directories for images and thumbnails
                Directory.CreateDirectory(newPath);
                Directory.CreateDirectory(thumbNewPath);

                // extracting the name of the image and appending it the new paths
                newPath = newPath+path.Substring(path.LastIndexOf("\\"));
                thumbNewPath = thumbNewPath+path.Substring(path.LastIndexOf("\\"));

                // save image as a thumbnail
                Image image = Image.FromFile(path);
                Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                // save the image in the correct dir in output dir
                image.Save(newPath);
                // save the thumbnail in the correct dir in the output dir
                thumb.Save(Path.ChangeExtension(thumbNewPath, "thumb"));
                // release the images from usage
                image.Dispose();
                thumb.Dispose();
                result = true;


                // TODO: think if we can send this class the logger to write that a new file was added


                return newPath;
            }
            // if file doesn't exist then return the correct message
            result = false;
            return "Source file doesn't exist";
        }
    }
    #endregion
}

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
    /// <summary>
    /// the class that is charge of operations on the images (creating folders, coping files, create thumbnails etc...)
    /// </summary>
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        // The Output Folder
        private string m_OutputFolder;
        // The Size Of The Thumbnail Size
        private int m_thumbnailSize;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name= path> the path of the file (image) we want to copy </param>
        /// <param name= size> the size of the thumbnail we create </param>
        public ImageServiceModal(string path, int size)
        {
            this.m_OutputFolder = path;
            this.m_thumbnailSize = size;
        }

         /// <summary>
        /// adds an image to output dir as a thumbnail 
        /// </summary>
        /// <param name= path> path to the file we want to copy </param>
        /// <param name= result> an indication if operation was successful or not </param>
        /// <return> returns the path of the new file if succeeded or and error message if failed </return>
        public string AddFile(string path, out bool result)
        {
            if(File.Exists(path)) {
                
                string newPath;
                string thumbNewPath;

                // create all directories needed if they don't already exist
                this.CreateDirectoryHierarchy(path, out newPath, out thumbNewPath);

                // extracting the name of the image and appending it the new paths
                newPath = newPath+path.Substring(path.LastIndexOf("\\"));
                thumbNewPath = thumbNewPath+path.Substring(path.LastIndexOf("\\"));

                // create and save images in their destenation folders
                this.SaveImages(path, newPath, thumbNewPath);

                result = true;


                // TODO: think if we can send this class the logger to write that a new file was added


                return newPath;
            }
            // if file doesn't exist then return the correct message
            result = false;
            return "Source file doesn't exist";
        }

        /// <summary>
        /// creating the directories hierarchy sorted by year and month
        /// </summary>
        /// <param name= path> the path to the file we want to copy </param>
        private void CreateDirectoryHierarchy(string path, out string newPath, out string thumbNewPath)
        {
                // create output dir if doesn't exist
                DirectoryInfo dirInfo = Directory.CreateDirectory(this.m_OutputFolder);
                // create as a hidden directory
                dirInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                // extract images creation date and time
                DateTime timeCreated = File.GetCreationTime(path);
                int year = timeCreated.Year;
                int month = timeCreated.Month;

                // make new path to output folder
                newPath = this.m_OutputFolder+"\\"+year.ToString()+"\\"+month.ToString();
                // make new path to thumbnail folder
                thumbNewPath = this.m_OutputFolder+"\\Thumbnails"+"\\"+year.ToString()+"\\"+month.ToString();
                // create directories for images and thumbnails
                Directory.CreateDirectory(newPath);
                Directory.CreateDirectory(thumbNewPath);
        }

        /// <summary>
        /// creates the images and thumbnails in the directories we created
        /// </summary>  
        /// <param name= path> the path to the file we want tot copy </param>
        /// <param name= newPath> the path to the destinated folder </param>
        /// <param name= thumbNewPath> the path to the destination folder of the thumbnail we create </param>
        private void SaveImages(string path, string newPath, string thumbNewPath)
        {
            // save image as a thumbnail
            Image image = Image.FromFile(path);
            Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
            // save the thumbnail in the correct dir in the output dir
            thumb.Save(Path.ChangeExtension(thumbNewPath, "thumb"));
            // save the image in the correct dir in the output dir
            image.Save(newPath);
                
            // release the images from usage
            image.Dispose();
            thumb.Dispose();
            
            //System.IO.File.Delete(path);
            // save the image in the correct dir in output dir
            //System.IO.Directory.Move(path, newPath);
                
        }
    }
    #endregion
}

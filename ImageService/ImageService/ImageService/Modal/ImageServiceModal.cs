﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ImageService.Modal
{
    /// <summary>
    /// the class that is charge of operations on the images (creating folders, coping files, create thumbnails etc...)
    /// </summary>
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        // The Output Folder
        private string OutputFolder;
        // The Size Of The Thumbnail Size
        private int thumbnailSize;
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name= path> the path of the file (image) we want to copy </param>
        /// <param name= size> the size of the thumbnail we create </param>
        public ImageServiceModal(string path, int size)
        {
            this.OutputFolder = path;
            this.thumbnailSize = size;
        }

         /// <summary>
        /// adds an image to output dir as a thumbnail 
        /// </summary>
        /// <param name= path> path to the file we want to copy </param>
        /// <param name= result> an indication if operation was successful or not </param>
        /// <return> returns the path of the new file if succeeded or and error message if failed </return>
        public string AddFile(string path, out bool result)
        {
            // check if given file still exists
            if(File.Exists(path)) {
                string newPath;
                string thumbNewPath;

                try
                {
                    // create all directories needed if they don't already exist
                    this.CreateDirectoryHierarchy(path, out newPath, out thumbNewPath);
                } catch(Exception e)
                {
                    result = false;
                    return "Failed creating directories requierd for new file. Error thrown: " + e.Message;
                }

                // extracting the name of the image and appending it the new paths
                newPath = newPath + path.Substring(path.LastIndexOf("\\"));
                thumbNewPath = thumbNewPath + path.Substring(path.LastIndexOf("\\"));
                
                string message;
                // create and save images in their destenation folders
                this.SaveImages(path, newPath, thumbNewPath, out message);

                // check if both the image and the thumbnail were created
                bool image = File.Exists(newPath);
                bool thumbImage = File.Exists(thumbNewPath);
                result = true;
                if(!image || !thumbImage) {
                    result = false;
                }
                return message;
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
            DateTime timeCreated = new DateTime();
            try
            {
                // extract images creation date and time
                //timeCreated = File.GetCreationTime(path);
                timeCreated = this.GetDateTakenFromImage(path);
            } catch(Exception e)
            {
                try {
                    timeCreated = this.GetDateCreatedFromImage(path);
                } catch(Exception e1) {
                    try {
                        // create directory for images without creation date.
                        Directory.CreateDirectory(path + "\\No Date");
                        Directory.CreateDirectory(path + "\\Thumbnails - No Date");
                        newPath = this.OutputFolder + "\\No Date";
                        thumbNewPath = this.OutputFolder + "\\Thumbnails - No Date";
                    } catch (Exception innerE)
                    {
                        Exception newException = new Exception("Couldnt create No Date directories. Exception thrown: " 
                            + innerE.Message);
                        throw newException;
                    }
                }
            }
            int year = timeCreated.Year;
            int month = timeCreated.Month;

            try {
                // create output dir if doesn't exist
                DirectoryInfo dirInfo = Directory.CreateDirectory(this.OutputFolder);
                // create as a hidden directory
                dirInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                // make new path to output folder
                newPath = this.OutputFolder+"\\"+year.ToString()+"\\"+month.ToString();
                // make new path to thumbnail folder
                thumbNewPath = this.OutputFolder+"\\Thumbnails"+"\\"+year.ToString()+"\\"+month.ToString();
                // create directories for images and thumbnails
                Directory.CreateDirectory(newPath);
                Directory.CreateDirectory(thumbNewPath);
            } catch (Exception e) 
            {
                Exception newException = new Exception("Couldnt create directories. Exception thrown: "
                    + e.Message);
                throw newException;
            }
        }

        /// <summary>
        /// creates the images and thumbnails in the directories we created
        /// </summary>  
        /// <param name= path> the path to the file we want tot copy </param>
        /// <param name= newPath> the path to the destinated folder </param>
        /// <param name= thumbNewPath> the path to the destination folder of the thumbnail we create </param>
        private void SaveImages(string path, string newPath, string thumbNewPath, out string message)
        {
            message = "Nothing done yet";
            try {
                Thread.Sleep(10);
                int fileCount = 0;
                // get the files extention
                string extension = Path.GetExtension(newPath);
                // get the files path without its extention
                string temp = newPath.Substring(0, newPath.Length - extension.Length);
                string final = temp;
                // check how many instances of the same file exist by the fileCount counter (if the files exists)
                while (File.Exists(temp + (fileCount > 0 ? "(" + fileCount.ToString() + ")" 
                    + extension : extension)))
                {
                    fileCount++;
                    final = temp + (fileCount > 0 ? ("(" + fileCount.ToString() + ")") : "");
                }
                // the new files path
                newPath = final + extension;

                message = "Couldnt move image to new location";
                // move wanted file to its new location
                File.Move(path, newPath);
                message = "Couldnt extract thumbnail from image";
                // extract a thumbnail from the image
                Image image = Image.FromFile(newPath),
                thumb = image.GetThumbnailImage(thumbnailSize, thumbnailSize, () => false, IntPtr.Zero);
                // change thumb path acoording to the original image
                thumbNewPath = thumbNewPath.Substring(0, thumbNewPath.Length - extension.Length);
                thumbNewPath = thumbNewPath + (fileCount > 0 ? ("(" + fileCount.ToString() + ")") 
                    + extension : extension);
                message = "Couldnt save thumbnail";
                // save the thumbnail image
                thumb.Save(thumbNewPath);
                // close connection to thumb image
                thumb.Dispose();
                // close connection to image
                image.Dispose();
            } catch (Exception e) {
                // return when the message is the error message
                return;
            }
            // save the new file created as a message
            message = newPath;       
        }

        //retrieves the datetime WITHOUT loading the whole image
        private DateTime GetDateTakenFromImage(string path)
        {
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
        }

        // OPTIONAL: for checking if a file is in use
        /*private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked
            return false;
        }*/
    }
    #endregion
}
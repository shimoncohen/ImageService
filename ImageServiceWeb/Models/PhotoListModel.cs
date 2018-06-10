using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ImageServiceWeb.WebEventArgs;

namespace ImageServiceWeb.Models
{

    /// <summary>
    /// The class of the Photo list model
    /// </summary>
    public class PhotoListModel
    {
        /// <summary>
        /// Gets or sets the photo path.
        /// </summary>
        /// <value>
        /// The photo path.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string PhotoPath { get; set; }

        [Required]
        [Display(Name = "PhotosList")]
        private List<Photo> PhotosList = new List<Photo>();

        public event EventHandler<PhotoCountEventArgs> GetPhotosNum;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoListModel()
        {
            RefreshList();
        }

        /// <summary>
        /// Refreshes the list. clears the current list and add all the photos in the directory (adds and removes photos)
        /// </summary>
        public void RefreshList()
        {
            // check if photo exists
            if(Directory.Exists(PhotoPath))
            {
                this.PhotosList.Clear();
                string[] photos = getPhotosPaths();
                string[] photosThumbnails = Directory.GetFiles(PhotoPath + "\\Thumbnails", "*.jpg", SearchOption.AllDirectories);
                List<Tuple<string, string>> joinedPaths = sortPaths(photos, photosThumbnails);
                foreach (Tuple<string, string> photo in joinedPaths)
                {
                    PhotosList.Add(new Photo(PhotoPath, photo.Item1, photo.Item2));
                }
                // update the num of pictures in the main page
                PhotoCountEventArgs photoCountEventArgs = new PhotoCountEventArgs(this.Length());
                this.GetPhotosNum?.Invoke(this, photoCountEventArgs);
            }
        }

        /// <summary>
        /// Gets the list of photos and invokes the event of number of photos changed.
        /// </summary>
        /// <returns>The list of photos</returns>
        public List<Photo> GetPhotos()
        {
            RefreshList();
            // update the num of pictures in the main page
            PhotoCountEventArgs photoCountEventArgs = new PhotoCountEventArgs(this.Length());
            this.GetPhotosNum?.Invoke(this, photoCountEventArgs);
            return PhotosList;
        }

        /// <summary>
        /// returns the size of the list
        /// </summary>
        /// <returns>The size of the list</returns>
        public int Length()
        {
            return PhotosList.Count;
        }

        /// <summary>
        /// Removes the photo from the list.
        /// </summary>
        /// <param name="photoToRemove">The photo to remove.</param>
        public void RemovePhoto(Photo photoToRemove)
        {
            foreach (Photo pic in PhotosList)
            {
                if (pic.PhotoPath.Equals(photoToRemove.PhotoPath))
                {
                    PhotosList.Remove(photoToRemove);
                    string photoToDelete = photoToRemove.GetFullPath();
                    // delete the photo
                    File.Delete(photoToDelete);
                    string thumbToDelete = photoToRemove.GetFullThumbPath();
                    // delete the thumbnail
                    File.Delete(thumbToDelete);
                    break;
                }
            }
            // update the num of pictures in the main page
            PhotoCountEventArgs photoCountEventArgs = new PhotoCountEventArgs(this.Length());
            this.GetPhotosNum?.Invoke(this, photoCountEventArgs);
        }

        /// <summary>
        /// Updates the path.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="args">The <see cref="PhotosEventArgs"/> instance containing the event data.</param>
        public void updatePath(object sender, PhotosEventArgs args)
        {
            this.PhotoPath = args.Path;
        }

        /// <summary>
        /// Gets the photos paths.
        /// </summary>
        /// <returns>the pathsw of the pthoto and thumbnail</returns>
        private string[] getPhotosPaths()
        {
            List<string> paths = new List<string>();
            string[] directories = Directory.GetDirectories(this.PhotoPath);
            foreach(string path in directories)
            {
                if(!Path.GetFileName(path).Equals("Thumbnails"))
                {
                    paths.AddRange(Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories));
                }
            }
            return paths.ToArray();
        }

        /// <summary>
        /// Sorts the paths.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <param name="thumbnailPaths">The thumbnail paths.</param>
        /// <returns></returns>
        private List<Tuple<string, string>> sortPaths(string[] paths, string[] thumbnailPaths)
        {
            List<Tuple<string, string>> joinedPaths = new List<Tuple<string, string>>();
            foreach (string path in paths)
            {
                foreach(string thumbPath in thumbnailPaths)
                {
                    if(Path.GetFileName(path).Equals(Path.GetFileName(thumbPath)))
                    {
                        joinedPaths.Add(new Tuple<string, string>(path, thumbPath));
                        break;
                    }
                }
            }
            return joinedPaths;
        }
    }
}
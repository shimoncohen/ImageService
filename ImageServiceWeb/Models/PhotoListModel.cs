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
    public class PhotoListModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string PhotoPath { get; set; }

        [Required]
        [Display(Name = "PhotosList")]
        private List<Photo> PhotosList = new List<Photo>();

        public event EventHandler<PhotoCountEventArgs> GetPhotosNum;

        public PhotoListModel()
        {
            RefreshList();
        }

        public void RefreshList()
        {
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

        public List<Photo> GetPhotos()
        {
            RefreshList();
            // update the num of pictures in the main page
            PhotoCountEventArgs photoCountEventArgs = new PhotoCountEventArgs(this.Length());
            this.GetPhotosNum?.Invoke(this, photoCountEventArgs);
            return PhotosList;
        }

        public int Length()
        {
            return PhotosList.Count;
        }

        public void RemovePhoto(Photo photoToRemove)
        {
            foreach (Photo pic in PhotosList)
            {
                if (pic.PhotoPath.Equals(photoToRemove.PhotoPath))
                {
                    PhotosList.Remove(photoToRemove);
                    string begining = "~/";
                    string photoToDelete = begining + photoToRemove.PhotoPath;
                    File.Delete(photoToDelete);
                    string thumbToDelete = begining + photoToRemove.ThumbPhotoPath + "/" +
                        photoToRemove.Year + "/" + photoToRemove.Month + "/" + photoToRemove.PhotoName;
                    File.Delete(thumbToDelete);
                    break;
                }
            }
            // update the num of pictures in the main page
            PhotoCountEventArgs photoCountEventArgs = new PhotoCountEventArgs(this.Length());
            this.GetPhotosNum?.Invoke(this, photoCountEventArgs);
        }

        public void updatePath(object sender, PhotosEventArgs args)
        {
            this.PhotoPath = args.Path;
        }

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
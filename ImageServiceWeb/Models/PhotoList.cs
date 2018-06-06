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
    public class PhotoList
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotoPath")]
        public string PhotoPath { get; set; }

        [Required]
        [Display(Name = "PhotosList")]
        private List<Photo> PhotosList = new List<Photo>();

        public event EventHandler<PhotoCountEventArgs> GetPhotosNum;

        public PhotoList()
        {
            RefreshList();
        }

        public void RefreshList()
        {
            if(Directory.Exists(PhotoPath))
            {
                string[] photos = Directory.GetFiles(PhotoPath + "\\Thumbnails", "*.jpg", SearchOption.AllDirectories);
                foreach(string photo in photos)
                {
                    PhotosList.Add(new Photo(photo));
                }
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
                    File.Delete(photoToRemove.PhotoPath);
                    //TODO: REMOVE PIC FROM FILE PATH
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
    }
}
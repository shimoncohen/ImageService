using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotoList
    {
        string photoPath;
        static List<Photo> photosList = new List<Photo>();

        public PhotoList(string path)
        {
            photoPath = path;
            RefreshList();
        }

        public void RefreshList()
        {
            if(Directory.Exists(photoPath))
            {
                string[] photos = Directory.GetFiles(photoPath + "/Thumbnails", "*.jpg", SearchOption.AllDirectories);
                foreach(string photo in photos)
                {
                    photosList.Add(new Photo(photo));
                }
            }
        }
    }
}
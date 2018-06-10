using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace ImageServiceWeb.Controllers
{
    /// <summary>
    /// The class that is the controller
    /// </summary>
    public class ImageServiceController : Controller
    {
        // members
        private static List<string> handlers = new List<string>();
        private static ConfigInfo configInfo = new ConfigInfo();
        private static PhotoListModel photoList = new PhotoListModel();
        private static LogsModel logsModel = new LogsModel();
        private static ImageServiceWebModel ImageServiceWebModel = new ImageServiceWebModel(photoList);
        private static Photo photoToView = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageServiceController()
        {
            // sign to the event that gets the num of photos when the number of photos is updated
            photoList.GetPhotosNum += ImageServiceWebModel.UpdatePhotosNum;
            configInfo.sendPath += photoList.updatePath;
            photoList.PhotoPath = configInfo.OutputDir;
            // get the list of photos
            photoList.RefreshList();
            while(configInfo.InfoReceived == false)
            {
                System.Threading.Thread.Sleep(50);
            }
            photoList.RefreshList();
        }

        // GET: First Page
        [HttpGet]
        public ActionResult ImageWebView()
        {
            return View(ImageServiceWebModel);
        }

        // the config view
        [HttpGet]
        public ActionResult ConfigView()
        {
            return View(configInfo);
        }

        // the logs view without filter
        [HttpGet]
        public ActionResult LogsView()
        {
            return View(logsModel);
        }

        // error view
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        // the logs view wih filter
        [HttpPost]
        public ActionResult LogsView(string filter)
        {
            try
            {
                logsModel.Filter = filter;
                return RedirectToAction("LogsView");
            }
            catch
            {
                return RedirectToAction("LogsView");
            }
        }

        // the phoos view
        [HttpGet]
        public ActionResult PhotosView()
        {
            photoList.PhotoPath = configInfo.OutputDir;
            photoList.RefreshList();
            return View(photoList);
        }

        [HttpGet]
        public ActionResult PhotoView()
        {
            try
            {
                return View(photoToView);
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // delete photo view
        [HttpGet]
        public ActionResult DeletePhotoView(string photoPath)
        {
            Photo photo = findPhoto(photoPath);
            return View(photo);
        }

        // remove handler view
        [HttpGet]
        public ActionResult RemoveHandler(string path)
        {
            HandlerModel handler = new HandlerModel(path);
            return View(handler);
        }

        // view of a specific photo
        public ActionResult PhotoToView(string path)
        {
            try
            {
                foreach (Photo photo in photoList.GetPhotos())
                {
                    if (photo.PhotoPath.Equals(path))
                    {
                        photoToView = photo;
                        return RedirectToAction("PhotoView");
                    }
                }
                return RedirectToAction("Error");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // delete a directory view
        public ActionResult Delete(string path)
        {
            int i = 0;
            foreach (DirectoryModel dir in configInfo.Handlers)
            {
                if (dir.DirPath.Equals(path))
                {
                    configInfo.SendCommandToServer(Infrastructure.Enums.CommandEnum.CloseCommand, path);
                    // remove the directory
                    configInfo.Handlers.RemoveAt(i);
                    return RedirectToAction("ConfigView");
                }
                i++;
            }
            return RedirectToAction("Error");
        }

        public ActionResult DeletePhoto(string photoPath)
        {
            photoList.PhotoPath = configInfo.OutputDir;
            List<Photo> tempPhotosList = photoList.GetPhotos();
            foreach (Photo pic in tempPhotosList)
            {
                if (pic.PhotoPath.Equals(photoPath))
                {
                    photoList.RemovePhoto(pic);
                    return RedirectToAction("PhotosView");
                }
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// The function returns a specific photo
        /// <paramref name="path">The path to the photo</paramref>
        /// </summary>
        /// <returns>A specific photo of the iven path</returns>
        private Photo findPhoto(string path)
        {
            List<Photo> tempPhotosList = photoList.GetPhotos();
            foreach (Photo pic in tempPhotosList)
            {
                if (pic.PhotoPath.Equals(path))
                {
                    return pic;
                }
            }
            return null;
        }
    }
}
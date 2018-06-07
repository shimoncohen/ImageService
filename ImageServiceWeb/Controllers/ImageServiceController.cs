using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace ImageServiceWeb.Controllers
{
    public class ImageServiceController : Controller
    {
        static List<string> handlers = new List<string>()
        {
            { "path1" },
            { "path2" },
            { "path3" },
            { "path1" },
            { "path2" },
            { "path3" },
            { "path1" },
            { "path2" },
            { "path3" },
            { "path1" },
            { "path2" },
            { "path3" },
            { "path1" },
            { "path2" },
            { "path3" },
            { "path1" },
            { "path2" },
            { "path3" },
            { "path1" },
            { "path2" },
            { "path3" }
        };
        private static ConfigInfo configInfo = new ConfigInfo();
        private static PhotoListModel photoList = new PhotoListModel();
        private static LogsModel logsModel = new LogsModel();
        private static ImageServiceWebModel ImageServiceWebModel = new ImageServiceWebModel(photoList);
        private static Photo photoToView = null;
        private static string pathPhotoToDelete;

        public ImageServiceController()
        {
            photoList.GetPhotosNum += ImageServiceWebModel.UpdatePhotosNum;
            configInfo.sendPath += photoList.updatePath;
            photoList.PhotoPath = configInfo.OutputDir;
            photoList.RefreshList();
        }

        // GET: First Page
        [HttpGet]
        public ActionResult ImageWebView()
        {
            return View(ImageServiceWebModel);
        }

        [HttpGet]
        public ActionResult ConfigView()
        {
            return View(configInfo);
        }

        [HttpGet]
        public ActionResult LogsView()
        {
            return View(logsModel);
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

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

        [HttpGet]
        public ActionResult DeletePhotoView(string photoPath)
        {
            Photo photo = findPhoto(photoPath);
            return View(photo);
        }

        public ActionResult beforeDelete(string photoPath)
        {
            pathPhotoToDelete = photoPath;
            return RedirectToAction("DeletePhotoView");
        }

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

        public ActionResult Delete(string path)
        {
            int i = 0;
            foreach (DirectoryModel dir in configInfo.Handlers)
            {
                if (dir.DirPath.Equals(path))
                {
                    configInfo.SendCommandToServer(Infrastructure.Enums.CommandEnum.CloseCommand, path);
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

        private Photo findPhoto(string path)
        {
            List<Photo> tempPhotosList = photoList.GetPhotos();
            foreach (Photo pic in tempPhotosList)
            {
                if (pic.PhotoPath.Equals(path))
                {
                    photoList.RemovePhoto(pic);
                    return pic;
                }
            }
            return null;
        }
    }
}
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
        static ConfigInfo configInfo;
        static PhotoList photoList;
        static LogsModel logsModel;
        static ImageServiceWebModel ImageServiceWebModel;
        static Photo photoToView = null;

        public ImageServiceController()
        {
            configInfo = new ConfigInfo();
            photoList = new PhotoList();
            logsModel = new LogsModel();
            ImageServiceWebModel = new ImageServiceWebModel(photoList);
            photoList.GetPhotosNum += ImageServiceWebModel.UpdatePhotosNum;
        }

        // GET: First Page
        [HttpGet]
        public  ActionResult ImageWebView()
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

        /*[HttpGet]
        public JObject GetServiceInfo()
        {
            JObject data = new JObject();
            data["ServiceStatus"] = status;
            data["NumOfImages"] = numOfPics;
            return data;
        }

        [HttpGet]
        public JObject GetConfigInfo()
        {
            JObject data = new JObject();
            data["SourceName"] = configInfo.SourceName;
            data["LogName"] = configInfo.LogName;
            data["OutputDirectory"] = configInfo.OutputDir;
            data["ThumbnailSize"] = configInfo.ThumbnailSize;
            return data;
        }*/

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

        public ActionResult DeletePhoto(Photo photo)
        {
            int i = 0;
            List<Photo> tempPhotosList = photoList.GetPhotos();
            foreach (Photo pic in tempPhotosList)
            {
                if (pic.PhotoPath.Equals(photo.PhotoPath))
                {
                    photoList.RemovePhoto(photo);
                    return RedirectToAction("PhotoView");
                }
                i++;
            }
            return RedirectToAction("Error");
        }
    }
}
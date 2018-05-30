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
        static List<Directory> directories = new List<Directory>();
        private string status;
        private int numOfPics;

        // GET: First Page
        [HttpGet]
        public  ActionResult ImageWeb()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Config()
        {
            return View();
        }

        [HttpGet]
        public JObject GetServiceInfo()
        {
            JObject data = new JObject();
            data["ServiceStatus"] = status;
            data["NumOfImages"] = numOfPics;
            return data;
        }

        public ActionResult Delete(string path)
        {
            int i = 0;
            foreach (Directory dir in directories)
            {
                if (dir.DirPath.Equals(path))
                {
                    directories.RemoveAt(i);
                    return RedirectToAction("ConfigView");
                }
                i++;
            }
            return RedirectToAction("Error");
        }
    }
}
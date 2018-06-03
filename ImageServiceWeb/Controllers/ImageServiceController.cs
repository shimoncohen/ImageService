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
            { "path3" }
        };
        static ConfigInfo configInfo = new ConfigInfo(handlers, "out", "source", "log", 120);
        private string status;
        private int numOfPics;

        // GET: First Page
        [HttpGet]
        public  ActionResult ImageWebView()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConfigView()
        {
            return View(configInfo);
        }

        [HttpGet]
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
        }

        public ActionResult Delete(string path)
        {
            int i = 0;
            foreach (Directory dir in configInfo.Handlers)
            {
                if (dir.DirPath.Equals(path))
                {
                    configInfo.Handlers.RemoveAt(i);
                    return RedirectToAction("ConfigView");
                }
                i++;
            }
            return RedirectToAction("Error");
        }
    }
}
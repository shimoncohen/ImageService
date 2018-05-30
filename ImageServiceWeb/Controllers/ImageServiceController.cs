using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ImageServiceController : Controller
    {
        // GET: First Page
        public  ActionResult ImageWeb()
        {
            return View();
        }

        public ActionResult Config()
        {
            return View();
        }
    }
}
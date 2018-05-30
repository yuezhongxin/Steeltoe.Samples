using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simple4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string ConfigServer()
        {
            var config = ApplicationConfig.Configuration;
            var section = config.GetSection("info");

            return $"name: {section["name"]}, password: {section["password"]}, profile: {section["profile"]}";
        }

        public ActionResult Reload()
        {
            if (ApplicationConfig.Configuration != null)
            {
                ApplicationConfig.Configuration.Reload();
            }

            return View();
        }
    }
}
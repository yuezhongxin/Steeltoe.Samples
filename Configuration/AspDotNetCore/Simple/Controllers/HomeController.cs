using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Microsoft.Extensions.Configuration;
using Simple.Model;
using System;

namespace Simple.Controllers
{
    public class HomeController : Controller
    {
        private IOptionsSnapshot<ConfigServerData> IConfigServerData { get; set; }

        private IConfigurationRoot Config { get; set; }

        public HomeController(IConfigurationRoot config, IOptionsSnapshot<ConfigServerData> configServerData)
        {
            // The ASP.NET DI mechanism injects the data retrieved from the Spring Cloud Config Server 
            // as an IOptionsSnapshot<ConfigServerData>. This happens because we added the call to:
            // "services.Configure<ConfigServerData>(Configuration);" in the StartUp class
            if (configServerData != null)
                IConfigServerData = configServerData;

            Config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public ConfigServerData ConfigServer()
        {
            var data = IConfigServerData.Value;
            return data;
        }

        //public string ConfigServer2()
        //{
        //    var config = Startup.Configuration;
        //    var section = config.GetSection("info");

        //    return $"name: {section["name"]}, password: {section["password"]}, profile: {section["profile"]}";
        //}

        public IActionResult Reload()
        {
            if (Config != null)
            {
                Config.Reload();
            }

            return View();
        }
    }
}

using System;
using System.Web.Mvc;
using NLog;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            Logger.Info("In the Index action...");
            return View();
        }

        [HttpPost]
        public ActionResult ThrowError()
        {
            throw new Exception("Oh no!!");
        }
    }
}

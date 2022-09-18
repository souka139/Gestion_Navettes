using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Navettes.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Connexion()
        {
            return RedirectToAction("index", "Connexion"); //ActionName,controllerName
        }
    }
}
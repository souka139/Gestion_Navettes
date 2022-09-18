using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Navettes.Models;

namespace Gestion_Navettes.Controllers
{
    public class abonnesRegisterController : Controller
    {
        private gestionNavetteEntities1 db = new gestionNavetteEntities1();

        // GET: abonnesRegister
        public ActionResult Index()
        {
            return View(db.abonne.ToList());
        }

        public ActionResult connexion()
        {
            return RedirectToAction("Index", "Connexion");//ActionName,controllerName
        }


        // GET: abonnesRegister/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: abonnesRegister/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom_complet,email,telephone,login,mdp")] abonne abonne)
        {
            if (ModelState.IsValid)
            {
                db.abonne.Add(abonne);
                db.SaveChanges();
                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index", "Connexion");//ActionName,controllerName
            }

            return View(abonne);
        }

    }
}

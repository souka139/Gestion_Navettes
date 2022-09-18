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
    [RoutePrefix("admin/villes")]
    public class villesController : Controller
    {
        private gestionNavetteEntities1 db = new gestionNavetteEntities1();

        // GET: villes
        [Route]
        public ActionResult Index()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            return View(db.ville.ToList());
        }

        // GET: villes/Create
        [Route("ajouter")]
        public ActionResult Create()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            return View();
        }

        // POST: villes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajouter")]
        public ActionResult Create([Bind(Include = "id,nom_ville")] ville ville)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (ModelState.IsValid)
            {
                db.ville.Add(ville);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ville);
        }

    }
}
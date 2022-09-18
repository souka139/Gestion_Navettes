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
    public class EspaceAbonneeController : Controller
    {
        private gestionNavetteEntities1 db = new gestionNavetteEntities1();

        // GET: EspaceAbonnee
        public ActionResult Index()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            var abonnement = db.abonnement.Include(a => a.autocar).Include(a => a.ville).Include(a => a.ville1);
            return View(abonnement.ToList());
        }

        public ActionResult details(int? id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            var autocar = db.autocar.Find(id);
            return View(autocar);
        }

        // GET: EspaceAbonnee/Create
        public ActionResult Create(int? id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            int abonneCourante = ((abonne)Session["abonne"]).id;

            ViewBag.id_abonne= new SelectList(db.abonne.Where(s => s.id == abonneCourante), "id", "id");
            ViewBag.id_abonnement = new SelectList(db.abonnement.Where(c =>c.id==id), "id", "id");
            return View();
        }

        // POST: EspaceAbonnee/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_abonne,id_abonnement")] abonne_abonnement abonne_Abonnement)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            int id = ((abonne)Session["abonne"]).id;
            int a = (from x in db.abonne_abonnement where x.id_abonnement == abonne_Abonnement.id_abonnement && abonne_Abonnement.id_abonne==id select x).Count();
            if (ModelState.IsValid)
            {
                if (a == 1)
                {
                    TempData["msg"] = "Vous pouvez s'abonner une seule fois !!";
                    return RedirectToAction("Index");
                }
                else
                {
                    db.abonne_abonnement.Add(abonne_Abonnement);
                    db.SaveChanges();

                    abonnement abonnement = (from c in db.abonnement where c.id == abonne_Abonnement.id_abonnement select c).FirstOrDefault();
                    abonnement.nbr_abonne_voulu--;
                    TempData["success"] = "Operation effectué avec succés !";
                    db.SaveChanges();

                    return RedirectToAction("Abonnements");
                }

            }

            ViewBag.id_abonne = new SelectList(db.abonne.Where(s => s.id == id), "id", "id");
            ViewBag.id_abonnement = new SelectList(db.abonnement, "id", "id");
            return View(abonne_Abonnement);
        }

        public ActionResult Abonnements()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            abonne abonneeCourante = (abonne)Session["abonne"];

            var req = (from s in db.abonnement
                       join c in db.abonne_abonnement on s.id equals c.id_abonnement
                       where c.id_abonne == abonneeCourante.id
                       select s).ToList();
            return View(req);
        }

    }
}

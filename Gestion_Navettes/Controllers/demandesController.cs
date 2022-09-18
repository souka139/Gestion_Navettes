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
    public class demandesController : Controller
    {
        private gestionNavetteEntities1 db = new gestionNavetteEntities1();

        // GET: demandes
        // pour l'abonnee courant
        public ActionResult Index()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            abonne abonneeCourante = (abonne)Session["abonne"];

            var req = (from s in db.demande
                       where s.id_abonne == abonneeCourante.id
                       select s).ToList();
            return View(req);
        }
        
        
        //pour que les abonnees interresse a une demande
        public ActionResult voirTout()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            // l'abonnee courant ne peut pas s'interresser a son demande
            int abonneCourante = ((abonne)Session["abonne"]).id;

            var demande = (from s in db.demande where s.id_abonne != abonneCourante select s).ToList();

            return View(demande);
        }

        public ActionResult btnInterress(int? id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            demande demande = db.demande.Find(id);
            if (demande == null)
            {
                return HttpNotFound();
            }

            ViewBag.id = new SelectList(db.demande.Where(c=>c.id == id), "id", "id");
            return View(demande);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnInterress([Bind(Include = "id")] demande d)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            if (ModelState.IsValid)
            {
                demande demande = (from c in db.demande where c.id == d.id select c).FirstOrDefault();
                demande.nbr_clients_inters++;
                TempData["msg"] = "Operation effectué avec succés !";
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.demande.Where(c=>c.id==d.id), "id", "id");
            return View(d);

        }


        // GET: demandes/Create
        public ActionResult Create()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            int id = ((abonne)Session["abonne"]).id;

            ViewBag.id_abonne = new SelectList(db.abonne.Where(s => s.id == id), "id", "nom_complet");
            ViewBag.id_ville_depart = new SelectList(db.ville, "id", "nom_ville");
            ViewBag.id_ville_arrivee = new SelectList(db.ville, "id", "nom_ville");

            return View();
        }

        // POST: demandes/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_abonne,id_ville_depart,id_ville_arrivee,date_debut,date_fin,heure_depart,heure_arrivee,nbr_clients_inters,status_demande")] demande demande)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (ModelState.IsValid)
            {
                db.demande.Add(demande);
                db.SaveChanges();
                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index");
            }

            ViewBag.id_abonne = new SelectList(db.abonne, "id", "nom_complet", demande.id_abonne);
            ViewBag.id_ville_depart = new SelectList(db.ville, "id", "nom_ville");
            ViewBag.id_ville_arrivee = new SelectList(db.ville, "id", "nom_ville");

            return View(demande);
        }

        // GET: demandes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            demande demande = db.demande.Find(id);
            if (demande == null)
            {
                return HttpNotFound();
            }
            int i = ((abonne)Session["abonne"]).id;

            ViewBag.id_abonne = new SelectList(db.abonne.Where(s => s.id == i), "id", "nom_complet");
            ViewBag.id_ville_depart = new SelectList(db.ville, "id", "nom_ville",demande.id_ville_depart);
            ViewBag.id_ville_arrivee = new SelectList(db.ville, "id", "nom_ville", demande.id_ville_arrivee);
            return View(demande);
        }

        // POST: demandes/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_abonne,id_ville_depart,id_ville_arrivee,date_debut,date_fin,heure_depart,heure_arrivee,nbr_clients_inters,status_demande")] demande demande)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (ModelState.IsValid)
            {
                db.Entry(demande).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index");
            }
            ViewBag.id_abonne = new SelectList(db.abonne, "id", "nom_complet", demande.id_abonne);
            ViewBag.id_ville_depart = new SelectList(db.ville, "id", "nom_ville",demande.heure_depart);
            ViewBag.id_ville_arrivee = new SelectList(db.ville, "id", "nom_ville", demande.id_ville_arrivee);
            return View(demande);
        }

        // GET: demandes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            demande demande = db.demande.Find(id);
            if (demande == null)
            {
                return HttpNotFound();
            }
            return View(demande);
        }

        // POST: demandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            demande demande = db.demande.Find(id);
            db.demande.Remove(demande);
            db.SaveChanges();
            TempData["msg"] = "Operation effectué avec succés !";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

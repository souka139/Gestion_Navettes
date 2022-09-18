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
    [RoutePrefix("societe")]
    public class EspaceSocieteController : Controller
    {
        // l'objet qui represente la base
        private gestionNavetteEntities1 db =
                    new gestionNavetteEntities1();

        // GET: EspaceSociete
        [Route]
        public ActionResult Index()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            societe societeCourante = (societe)Session["societe"];

            /*var req = db.abonnements
                    .Where(s => s.autocar.id_societe == societeCourante.id);*/

            var req = (from s in db.abonnement
                       where s.autocar.id_societe == societeCourante.id
                       select s).ToList();

            return View(req);
        }

        [Route("ajouter")]
        public ActionResult create()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            int id = ((societe)Session["societe"]).id;

            ViewBag.id_autocar = new SelectList(db.autocar.Where(s => s.id_societe == id), "id", "id");
            ViewBag.id_ville_depart = new SelectList(db.ville, "id", "nom_ville");
            ViewBag.id_ville_arrivee = new SelectList(db.ville, "id", "nom_ville");

            return View();
        }

        [HttpPost]
        [Route("ajouter")]
        public ActionResult create(abonnement ab)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (ModelState.IsValid)
            {
                db.abonnement.Add(ab);
                db.SaveChanges();
                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index");
            }

            int id = ((societe)Session["societe"]).id;

            ViewBag.id_autocar = new SelectList(db.autocar.Where(s => s.id_societe == id), "id", "id");
            ViewBag.id_ville_depart = new SelectList(db.ville, "id", "nom_ville");
            ViewBag.id_ville_arrivee = new SelectList(db.ville, "id", "nom_ville");
            return View();
        }

        [Route("voirDetails/{id}")]
        public ActionResult details(int id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            var autocar = db.autocar.Find(id);
            return View(autocar);
        }


        //pour les societes
        public ActionResult demandes()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            var demande = db.demande.Include(d => d.abonne);
            var demandes = db.demande.Where(d => d.status_demande!="accepted");
            return View(demandes.ToList());
        }

        public ActionResult gereDemande(int? id)
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
            societe societeCourante = (societe)Session["societe"];
                
            ViewBag.id_autocar = new SelectList(db.autocar.Where(s => s.id_societe == societeCourante.id), "id", "id");
            ViewBag.id_ville_depart = new SelectList(db.ville.Where(c=>c.id==demande.id_ville_depart), "id", "nom_ville");
            ViewBag.id_ville_arrivee = new SelectList(db.ville.Where(c => c.id == demande.id_ville_arrivee), "id", "nom_ville");

            return View(demande);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult gereDemande([Bind(Include = "id,id_ville_depart,id_ville_arrivee,date_debut,date_fin,heure_depart,heure_arrivee,prix_abonnement,id_autocar,nbr_abonne_voulu")] abonnement abonnement,demande demande)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
           
            if (ModelState.IsValid)
            {
                //ajouter la demande d'abonnement dans la liste des offres 
                db.abonnement.Add(abonnement);
                db.SaveChanges();
                //actualiser le status de demande
                demande dd = (from f in db.demande where f.id==demande.id select f).FirstOrDefault();
                dd.status_demande = "accepted";
                db.Entry(dd).State = EntityState.Modified;
                db.SaveChanges();

                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index");
            }
            societe societeCourante = (societe)Session["societe"];

            ViewBag.id_autocar = new SelectList(db.autocar.Where(s => s.id_societe == societeCourante.id), "id", "id");
            ViewBag.id_ville_depart = new SelectList(db.ville.Where(c=>c.id==demande.id_ville_depart), "id", "nom_ville");
            ViewBag.id_ville_arrivee = new SelectList(db.ville.Where(c => c.id == demande.id_ville_arrivee), "id", "nom_ville");
            return View(abonnement);
        }



    }
}
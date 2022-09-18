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
    public class EspaceAdminController : Controller
    {
        private gestionNavetteEntities1 db = new gestionNavetteEntities1();

        // GET: EspaceAdmin
        public ActionResult Index()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            return View(db.societe.ToList());
        }
        
        public ActionResult abonnes()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            return View(db.abonne.ToList());
        } 
        
        public ActionResult demandes()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            return View(db.demande.ToList());
        } 
        
        public ActionResult abonnements()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            return View(db.abonnement.ToList());
        }

        //Delete societe
        // GET: EspaceAdmin/Delete/5
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
            societe societe = db.societe.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            societe societe = db.societe.Find(id);
            autocar autocar = (from d in db.autocar where d.id_societe==id select d).FirstOrDefault();
            abonnement abonnement = (from a in db.abonnement where a.autocar.id_societe == id select a).FirstOrDefault();

            if (autocar != null)
            {
                db.autocar.Remove(autocar);
            }
            db.abonnement.Remove(abonnement);
            db.societe.Remove(societe);
            db.SaveChanges();
            TempData["msg"] = "Operation effectué avec succés !";
            return RedirectToAction("Index");
        }
        
        //delete abonnee
        // GET: EspaceAdmin/Delete/5
        public ActionResult DeleteAbonne(int? id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            abonne abonne = db.abonne.Find(id);
            if (abonne == null)
            {
                return HttpNotFound();
            }
            return View(abonne);
        }

        [HttpPost, ActionName("DeleteAbonne")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAbonneConfirmed(int id)
        {
            abonne abonne = db.abonne.Find(id);
            abonne_abonnement abonne_Abonnement= (from d in db.abonne_abonnement where d.id_abonne == id select d).FirstOrDefault();
            demande demande= (from d in db.demande where d.id_abonne == id select d).FirstOrDefault();
            if(abonne_Abonnement != null)
            {
                db.abonne_abonnement.Remove(abonne_Abonnement);
            }
            if (demande != null)
            {
                db.demande.Remove(demande);
            }
            db.abonne.Remove(abonne);
            db.SaveChanges();
            TempData["msg"] = "Operation effectué avec succés !";
            return RedirectToAction("abonnes");
        }

        //Delete demande
        public ActionResult DeleteDemande(int? id)
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

        [HttpPost, ActionName("DeleteDemande")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDemandeConfirmed(int id)
        {
            demande demande = db.demande.Find(id);
            db.demande.Remove(demande);
            db.SaveChanges();
            TempData["msg"] = "Operation effectué avec succés !";
            return RedirectToAction("demandes");
        }

        //delete abonnement
        public ActionResult DeleteAbonnement(int? id)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            abonnement abonnement = db.abonnement.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        [HttpPost, ActionName("DeleteAbonnement")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAbonnementConfirmed(int id)
        {
            abonnement abonnement = db.abonnement.Find(id);
            abonne_abonnement abonne_Abonnement = (from a in db.abonne_abonnement where a.id_abonnement == id select a).FirstOrDefault();
            if (abonne_Abonnement != null)
            {
                db.abonne_abonnement.Remove(abonne_Abonnement);
            }
            db.abonnement.Remove(abonnement);
            db.SaveChanges();
            TempData["msg"] = "Operation effectué avec succés !";
            return RedirectToAction("abonnements");
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

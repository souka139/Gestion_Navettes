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
    [RoutePrefix("admin/autocars")]
    public class autocarsController : Controller
    {
        private gestionNavetteEntities1 db = new gestionNavetteEntities1();
        // GET: autocars
        public ActionResult Index()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            societe societeCourante = (societe)Session["societe"];

            var autocars = (from s in db.autocar
                       where s.id_societe == societeCourante.id
                       select s).ToList();

            return View(autocars);
        }

        // GET: autocars/Create
        [Route("ajouter")]
        public ActionResult Create()
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }


            ViewBag.id_societe = new SelectList(db.societe, "id", "nom");
            return View();
        }

        // POST: autocars/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ajouter")]
        public ActionResult Create([Bind(Include = "id,clim,capacite,id_societe")] autocar autocar)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (Session["societe"] != null)
            {
                ModelState.Remove("id_societe");

                int id_societe = ((societe)Session["societe"]).id;
                autocar.id_societe = id_societe;

                db.autocar.Add(autocar);
                db.SaveChanges();
                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index");
            }
            else
                if (ModelState.IsValid)
            {
                db.autocar.Add(autocar);
                db.SaveChanges();
                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index");
            }

            ViewBag.id_societe = new SelectList(db.societe, "id", "nom", autocar.id_societe);
            return View(autocar);
        }

        // GET: autocars/Edit/5
        [Route("modifier/{id}")]
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
            autocar autocar = db.autocar.Find(id);
            if (autocar == null)
            {
                return HttpNotFound();
            }
            int id_societe = ((societe)Session["societe"]).id;
            ViewBag.id_societe = new SelectList(db.societe.Where(a=>a.id==id_societe), "id", "nom", autocar.id_societe);
            return View(autocar);
        }

        // POST: autocars/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("modifier/{id}")]
        public ActionResult Edit([Bind(Include = "id,clim,capacite,id_societe")] autocar autocar)
        {
            if (Session["societe"] == null && Session["admin"] == null && Session["abonne"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }

            if (ModelState.IsValid)
            {
                db.Entry(autocar).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Operation effectué avec succés !";
                return RedirectToAction("Index");
            }
            ViewBag.id_societe = new SelectList(db.societe, "id", "nom", autocar.id_societe);
            return View(autocar);
        }

        // GET: autocars/Delete/5
        [Route("supprimer/{id}")]
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
            autocar autocar = db.autocar.Find(id);
            if (autocar == null)
            {
                return HttpNotFound();
            }
            return View(autocar);
        }

        // POST: autocars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("supprimerConfir/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            autocar autocar = db.autocar.Find(id);
            db.autocar.Remove(autocar);
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

using Gestion_Navettes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Gestion_Navettes.Controllers
{
    [RoutePrefix("seConnecter")]
    public class ConnexionController : Controller
    {
        // l'objet qui represente la base
        private gestionNavetteEntities1 db = new gestionNavetteEntities1();

        // GET: Connexion
        [Route]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Abonner()
        {
            return RedirectToAction("Create", "abonnesRegister"); //ActionName,controllerName
        }

        public ActionResult Societe()
        {
            return RedirectToAction("Create", "societesRegister"); //ActionName,controllerName
        }
       

        [Route]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(userRole user) // model binding
        {

            if (ModelState.IsValid)
            {

                //societe
                if (user.usertype == 1)
                {
                    var trouve = db.societe.Where(s => s.login == user.login &&
                                s.mdp == user.mdp).FirstOrDefault();
                    if (trouve != null)
                    {
                        // garder des traces
                        Session["societe"] = trouve;

                        // retourner vers son propre espace
                        return RedirectToAction("Index", "EspaceSociete");
                    }
                    else
                    {
                        ViewBag.nonTrouve = "Pas encore inscrit";
                        return View();
                    }
                }
                else // un abonne
                {
                    if (user.usertype == 2)
                    {
                        var trouve = db.abonne.Where(s => s.login == user.login &&
                                   s.mdp == user.mdp).FirstOrDefault();
                        if (trouve != null)
                        {
                            // garder des traces
                            Session["abonne"] = trouve;
                            // retourner vers son propre espace
                            return RedirectToAction("Index", "EspaceAbonnee");

                        }
                        else
                        {
                            ViewBag.nonTrouve = "Pas encore inscrit";
                            return View();
                        }
                    }
                    else
                    {
                        var trouve = db.administrateur.Where(s => s.login == user.login &&
                                   s.mdp == user.mdp).FirstOrDefault();
                        if (trouve != null)
                        {
                            Session["admin"] = trouve;
                            return RedirectToAction("Index", "EspaceAdmin");
                        }
                        else
                        {
                            ViewBag.nonTrouve = "Login ou password incorrect";
                            return View();
                        }
                    }
                }

            }
            return View();
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
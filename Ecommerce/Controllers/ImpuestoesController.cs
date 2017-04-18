using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class ImpuestoesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Impuestoes
        public ActionResult Index()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var impuestoes = db.Impuestoes.Where(x => x.CompanyID == user.CompanyID);
            return View(impuestoes.ToList());
        }

        // GET: Impuestoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Impuesto impuesto = db.Impuestoes.Find(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            return View(impuesto);
        }

        // GET: Impuestoes/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var impuesto = new Impuesto {CompanyID = user.CompanyID};
            return View(impuesto);
        }

        // POST: Impuestoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                db.Impuestoes.Add(impuesto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(impuesto);
        }

        // GET: Impuestoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var impuesto = db.Impuestoes.Find(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            
            return View(impuesto);
        }

        // POST: Impuestoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(impuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(impuesto);
        }

        // GET: Impuestoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Impuesto impuesto = db.Impuestoes.Find(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            return View(impuesto);
        }

        // POST: Impuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Impuesto impuesto = db.Impuestoes.Find(id);
            db.Impuestoes.Remove(impuesto);
            db.SaveChanges();
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

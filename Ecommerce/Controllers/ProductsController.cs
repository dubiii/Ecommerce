using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Clases;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class ProductsController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Products
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var products = db.Products.Include(p => p.Category).Include(p => p.Impuesto).Where(p => p.CompanyID == user.CompanyID);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CategoryID = new SelectList(comboHelper.GetCategories(user.CompanyID), "CategoryID", "Description");
            ViewBag.ImpuestoID = new SelectList(comboHelper.GetImpuestos(user.CompanyID), "ImpuestoID", "Description");
            var producto = new Product {CompanyID = user.CompanyID};
            return View(producto);
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();

                if (product.ImageFile != null)
                {
                    var folder = "~/Content/Products";
                    var file = string.Format("{0}.jpg", product.ProductID);
                    var response = fileHelper.UploadPhoto(product.ImageFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        product.Image = pic;
                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(comboHelper.GetCategories(user.CompanyID), "CategoryID", "Description");
            ViewBag.ImpuestoID = new SelectList(comboHelper.GetImpuestos(user.CompanyID), "ImpuestoID", "Description");
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(comboHelper.GetCategories(product.CompanyID), "CategoryID", "Description");
            ViewBag.ImpuestoID = new SelectList(comboHelper.GetImpuestos(product.CompanyID), "ImpuestoID", "Description");
            return View(product);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Products";
                    var file = string.Format("{0}.jpg", product.ProductID);
                    var response = fileHelper.UploadPhoto(product.ImageFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        product.Image = pic;
                    }
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(comboHelper.GetCategories(product.CompanyID), "CategoryID", "Description");
            ViewBag.ImpuestoID = new SelectList(comboHelper.GetImpuestos(product.CompanyID), "ImpuestoID", "Description");
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
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

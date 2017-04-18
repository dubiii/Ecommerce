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
    public class WarehousesController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Warehouses
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var warehouses = db.Warehouses.Where(w => w.CompanyID == user.CompanyID).Include(w => w.Cities).Include(w => w.Departments);
            return View(warehouses.ToList());
        }

        // GET: Warehouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // GET: Warehouses/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CityID = new SelectList(comboHelper.GetCities(0), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            var warehouse = new Warehouse {CompanyID = user.CompanyID};
            return View(warehouse);
        }

        // POST: Warehouses/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Warehouses.Add(warehouse);
                var response = dbHelper.saveChanges(db);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }
                
                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewBag.CityID = new SelectList(comboHelper.GetCities(warehouse.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(comboHelper.GetCities(warehouse.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(comboHelper.GetCities(warehouse.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var warehouse = db.Warehouses.Find(id);
            db.Warehouses.Remove(warehouse);
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

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
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Cities).Include(u => u.Companies).Include(u => u.Departments);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(comboHelper.GetCities(0), "CityID", "Name");
            ViewBag.CompanyID = new SelectList(comboHelper.GetCompanies(), "CompanyID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges(); //pa despues rodearlo con try catch OJO!! a validar registros duplicados y registros relacionados AL BORRAR!!!

                userHelper.CreateUserASP(user.UserName, "User");

                if (user.PhotoFile != null)
                {
                    //var pic = string.Empty;
                    var folder = "~/Content/Users";
                    var file = string.Format("{0}.jpg", user.UserID);
                    var response = fileHelper.UploadPhoto(user.PhotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        user.Photo = pic;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }

                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(comboHelper.GetCities(user.DepartmentID), "CityID", "Name");
            ViewBag.CompanyID = new SelectList(comboHelper.GetCompanies(), "CompanyID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(comboHelper.GetCities(user.DepartmentID), "CityID", "Name");
            ViewBag.CompanyID = new SelectList(comboHelper.GetCompanies(), "CompanyID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(user);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.PhotoFile != null)
                {
                    var file = string.Format("{0}.jpg", user.UserID);
                    var folder = "~/Content/Users";
                    var response = fileHelper.UploadPhoto(user.PhotoFile, folder, file);
                    user.Photo = string.Format("{0}/{1}", folder, file);
                }

                var db2 = new EcommerceContext();
                var currentUser = db2.Users.Find(user.UserID);

                if (currentUser.UserName != user.UserName)
                {
                    userHelper.updateUserName(currentUser.UserName, user.UserName);
                }

                db2.Dispose();

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            ViewBag.CityID = new SelectList(comboHelper.GetCities(user.DepartmentID), "CityID", "Name");
            ViewBag.CompanyID = new SelectList(comboHelper.GetCompanies(), "CompanyID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            userHelper.deleteUser(user.UserName, "User");
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

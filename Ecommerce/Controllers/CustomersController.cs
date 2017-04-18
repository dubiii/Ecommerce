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
    public class CustomersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        // GET: Customers
        public ActionResult Index()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerID equals cc.CustomerID
                       join co in db.Companies on cc.CompanyID equals co.CompanyID
                       where co.CompanyID == user.CompanyID
                       select new { cu }).ToList();


            var customers = new List<Customer>();
            foreach (var item in qry)
            {
                customers.Add(item.cu);
            }

            
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CityID = new SelectList(comboHelper.GetCities(0), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View();
        }

        // POST: Customers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Customers.Add(customer);
                        var response = dbHelper.saveChanges(db);
                        if (!response.Succeded)
                        {
                            ModelState.AddModelError(string.Empty, response.Message);
                            transaction.Rollback();
                        }
                        userHelper.CreateUserASP(customer.UserName, "Customer");

                        var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                        var companyuser = new CompanyCustomer { CompanyID = user.CompanyID, CustomerID = customer.CustomerID};
                        db.CompanyCustomers.Add(companyuser);
                        db.SaveChanges();
                        transaction.Commit();
                        if (customer.PhotoFile != null)
                        {
                            //var pic = string.Empty;
                            var folder = "~/Content/Customers";
                            var file = string.Format("{0}.jpg", customer.CustomerID);
                            var respo = fileHelper.UploadPhoto(customer.PhotoFile, folder, file);
                            if (respo)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                customer.Photo = pic;
                                db.Entry(customer).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                        return RedirectToAction("Index");
                        
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.CityID = new SelectList(comboHelper.GetCities(customer.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(comboHelper.GetCities(customer.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(customer);
        }

        // POST: Customers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.PhotoFile != null)
                {
                    var file = string.Format("{0}.jpg", customer.CustomerID);
                    var folder = "~/Content/Customers";
                    var resp = fileHelper.UploadPhoto(customer.PhotoFile, folder, file);
                    customer.Photo = string.Format("{0}/{1}", folder, file);
                }

                var db2 = new EcommerceContext();
                var currentCustomer = db2.Users.Find(customer.CustomerID);

                if (currentCustomer.UserName != customer.UserName)
                {
                    userHelper.updateUserName(currentCustomer.UserName, customer.UserName);
                }

                db2.Dispose();


                db.Entry(customer).State = EntityState.Modified;
                var response = dbHelper.saveChanges(db);
                if (response.Succeded)
                {
                    //TODO: validar cuando el correo del customer cambie
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            ViewBag.CityID = new SelectList(comboHelper.GetCities(customer.DepartmentID), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(comboHelper.GetDepartments(), "DepartmentID", "Name");
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var companyCustomer = db.CompanyCustomers.Where(cc => cc.CompanyID == user.CompanyID && cc.CustomerID == customer.CustomerID).FirstOrDefault();
            using (var transaction = db.Database.BeginTransaction())
            {
                db.CompanyCustomers.Remove(companyCustomer);
                db.Customers.Remove(customer);
                var response = dbHelper.saveChanges(db);
                //if (response.Succeded)
                //{
                //    ModelState.AddModelError(string.Empty, response.Message);
                //    userHelper.deleteUser(customer.UserName, "Customer");
                //}

                if (response.Succeded)
                {
                    transaction.Commit();
                    return RedirectToAction("Index");
                }

                transaction.Rollback();
                ModelState.AddModelError(string.Empty, response.Message);
                return View(customer); 
            }
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

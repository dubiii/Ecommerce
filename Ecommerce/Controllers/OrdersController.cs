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
    public class OrdersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();

        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var remove = db.OrderDetailTemps.Where(o => o.UserName == User.Identity.Name && o.ProductID == id).FirstOrDefault();
            if (remove == null)
            {
                return HttpNotFound();
            }

            db.OrderDetailTemps.Remove(remove);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        public ActionResult AddProduct()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductID = new SelectList(comboHelper.GetProducts(user.CompanyID, true), "ProductID", "Description");
            return PartialView();
        }

        //POST: Orders

        [HttpPost]
        public ActionResult AddProduct(AddProductView view)
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var orderDetailTemp = db.OrderDetailTemps.Where(o => o.UserName == User.Identity.Name && o.ProductID == view.ProductID).FirstOrDefault();
                if (orderDetailTemp == null)
                {
                    var product = db.Products.Find(view.ProductID);
                    orderDetailTemp = new OrderDetailTemp
                    {
                        Description = product.Description,
                        Price = product.Price,
                        ProductID = product.ProductID,
                        Quantity = view.Quantity,
                        ImpuestoRate = product.Impuesto.Rate,
                        UserName = User.Identity.Name,
                    };
                    db.OrderDetailTemps.Add(orderDetailTemp);
                }
                else
                {
                    orderDetailTemp.Quantity += view.Quantity;
                    db.Entry(orderDetailTemp).State = EntityState.Modified;
                }
                
                
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewBag.ProductID = new SelectList(comboHelper.GetProducts(user.CompanyID), "ProductID", "Description");
            return PartialView(view);
        }

        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            var orders = db.Orders.Where(o => o.CompanyID == user.CompanyID).Include(o => o.Customer).Include(o => o.State);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerID = new SelectList(comboHelper.GetCustomers(user.CompanyID), "CustomerID", "FullName");
            var view = new NewOrderView
            {
                Date = DateTime.Now,
                Details = db.OrderDetailTemps.Where(o => o.UserName == User.Identity.Name).ToList(),
            };
            return View(view);
        }

        // POST: Orders/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewOrderView view)
        {
            if (ModelState.IsValid)
            {
                var response = movementHelper.newOrder(view, User.Identity.Name);
                if (response.Succeded)
                {
                    return RedirectToAction("Index");
                }
                
                ModelState.AddModelError(String.Empty, response.Message);
            }

            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerID = new SelectList(comboHelper.GetCustomers(user.CompanyID), "CustomerID", "FullName");
            view.Details = db.OrderDetailTemps.Where(o => o.UserName == User.Identity.Name).ToList();
            return View(view);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.StateID = new SelectList(db.States, "StateID", "Description", order.StateID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,StateID,Date,Remarks")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.StateID = new SelectList(db.States, "StateID", "Description", order.StateID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private EcommerceContext db = new EcommerceContext();
        public ActionResult Index()
        {
            var userlogueado = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault(); 
            return View(userlogueado);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Ecommerce.Clases;
using Ecommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;

namespace Ecommerce.Controllers.API
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private EcommerceContext db = new EcommerceContext();

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(JObject form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var email = string.Empty;
            var password = string.Empty;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
                password = jsonObject.Password.Value;
            }
            catch
            {
                return BadRequest("Llamada Incorrecta");
            }

            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.Find(email, password);

            if (userASP == null)
            {
                return BadRequest("Usuario o Contraseña Incorrectos");
            }

            var user = db.Users.Where(u => u.UserName == email)
                .Include(u => u.Cities)
                .Include(u => u.Departments)
                .Include(u => u.Companies).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Usuario o Contraseña Incorrectos");
            }

            var userResponse = new UserResponse
            {
                Address = user.Address,
                CityId = user.CityID,
                CityName = user.Cities.Name,
                DepartmentId = user.DepartmentID,
                DepartmentName = user.Departments.Name,
                CompanyId = user.CompanyID,
                Company = new Company
                {
                    CompanyID = user.CompanyID,
                    Name = user.Companies.Name,
                    Phone = user.Companies.Phone,
                    Address = user.Companies.Address,
                    Logo = user.Companies.Logo,
                    LogoFile = user.Companies.LogoFile,
                    DepartmentID = user.Companies.DepartmentID,
                    CityID = user.Companies.CityID,
                },
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = userManager.IsInRole(userASP.Id, "Admin"),
                IsCustomer = userManager.IsInRole(userASP.Id, "Customer"),
                IsSupplier = userManager.IsInRole(userASP.Id, "Supplier"),
                IsUser = userManager.IsInRole(userASP.Id, "User"),
                Phone = user.Phone,
                Photo = user.Photo,
                UserId = user.UserID,
                UserName = user.UserName
            };
            


            return Ok(userResponse);
        }

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}
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

namespace Ecommerce.Controllers.API
{
    [RoutePrefix("api/Customers")]
    public class CustomersController : ApiController
    {
        private EcommerceContext db = new EcommerceContext();

        
        [HttpGet]
        [Route("Customers")]
        public IEnumerable<CustomerResponse> GetCustomer()
        {
            //db.Configuration.ProxyCreationEnabled = false;
            List<CustomerResponse> clientes = new List<CustomerResponse>();
            var customers = db.Customers.Include(c => c.Department).Include(c => c.City);
            

            foreach (var customer in customers)
            {
                //var companyCustomers = db.CompanyCustomers.Where(u => u.CustomerID == customer.CustomerID).ToList();
                //var orders = db.Orders.Where(o => o.CustomerID == customer.CustomerID).ToList();

                clientes.Add(new CustomerResponse
                {
                    CustomerID = customer.CustomerID,
                    UserName = customer.UserName,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    DepartmentID = customer.DepartmentID,
                    CityID = customer.CityID,
                    Department = new Department
                    {
                        DepartmentID = customer.Department.DepartmentID,
                        Name = customer.Department.Name
                    },
                    City = new City
                    {
                        CityID = customer.City.CityID,
                        Name = customer.City.Name
                    },
                    //CompanyCustomers = new List<CompanyCustomer>().Add(companyCustomers),
                });
            }
            return clientes;

        }

        // GET: api/Customers
        public IQueryable<Customer> GetCustomers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Customers;
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerID }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerID == id) > 0;
        }
    }
}
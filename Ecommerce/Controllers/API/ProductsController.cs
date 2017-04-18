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
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private EcommerceContext db = new EcommerceContext();

        [HttpGet]
        [Route("Products")]
        public IEnumerable<ProductResponse> GetProduct()
        {
            List<ProductResponse> logins = new List<ProductResponse>();
            var products = db.Products.Include(p => p.Category).Include(p => p.Impuesto).Include(p => p.Company).Include(p => p.Inventories);
            foreach (var product in products)
            {
                logins.Add(new ProductResponse
                {
                    ProductID = product.ProductID,
                    Description = product.Description,
                    BarCode = product.BarCode,
                    Image = product.Image,
                    Price = product.Price,
                    Remarks = product.Remarks,
                    CompanyID = product.CompanyID,
                    //Inventory = new Inventory
                    //{
                    //    InventoryID = product.Inventories.
                    //},
                    Company = new Company
                    {
                        CompanyID = product.Company.CompanyID,
                        Name = product.Company.Name,
                        Phone = product.Company.Phone,
                        Address = product.Company.Address,
                        Logo = product.Company.Logo,
                        DepartmentID = product.Company.DepartmentID,
                        CityID = product.Company.CityID
                    },
                    CategoryID = product.CategoryID,
                    Category = new Category
                    {
                        CategoryID = product.Category.CategoryID,
                        Description = product.Category.Description,
                        CompanyID = product.Category.CategoryID
                    },
                    ImpuestoID = product.ImpuestoID,
                    Impuesto = new Impuesto
                    {
                        ImpuestoID = product.Impuesto.ImpuestoID,
                        Description = product.Impuesto.Description,
                        Rate = product.Impuesto.Rate,
                        CompanyID = product.Impuesto.CompanyID
                    },

                });
            }

            return logins;
        }

        // GET: api/Products
        //public IQueryable<Product> GetProducts()
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    return db.Products;
        //}

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}
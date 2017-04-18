using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;

namespace Ecommerce.Clases
{
    public class comboHelper : IDisposable
    {
        private static EcommerceContext db = new EcommerceContext();
        public static List<Department> GetDepartments()
        {
            var departments = db.Departments.ToList();
            departments.Add(new Department
            {
                DepartmentID = 0,
                Name = "[Seleccione un departamento..]",
            });
            return departments.OrderBy(d => d.Name).ToList();
        }

        public static List<Product> GetProducts(int companyId, bool sw)
        {
            var products = db.Products.Where(p => p.CompanyID == companyId).ToList();
            return products.OrderBy(p => p.Description).ToList();
        }


        internal static List<Product> GetProducts(int companyID)
        {
            var products = db.Products.Where(x => x.CompanyID == companyID).ToList();
            products.Add(new Product
            {
                ProductID = 0,
                Description = "[Seleccione un producto..]",
            });
            return products.OrderBy(d => d.Description).ToList();
        }

        public static List<City> GetCities(int departmentID)
        {
            var cities = db.Cities.Where(c => c.DepartmentID == departmentID).ToList();
            cities.Add(new City
            {
                CityID = 0,
                Name = "[Seleccione una ciudad..]",
            });
            return cities.OrderBy(d => d.Name).ToList();
        }

        public static List<Company> GetCompanies()
        {
            var companies = db.Companies.ToList();
            companies.Add(new Company
            {
                CompanyID = 0,
                Name = "[Seleccione una compañia..]",
            });
            return companies.OrderBy(d => d.Name).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public static List<Category> GetCategories(int companyId)
        {
            var categories = db.Categories.Where(x => x.CompanyID == companyId).ToList();
            categories.Add(new Category
            {
                CategoryID = 0,
                Description = "[Seleccione una categoria..]",
            });
            return categories.OrderBy(d => d.Description).ToList();
        }

        internal static List<Customer> GetCustomers(int companyID)
        {
            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerID equals cc.CustomerID
                       join co in db.Companies on cc.CompanyID equals co.CompanyID
                       where co.CompanyID == companyID
                       select new { cu }).ToList();


            var customers = new List<Customer>();
            foreach (var item in qry)
            {
                customers.Add(item.cu);
            }
            customers.Add(new Customer
            {
                CustomerID = 0,
                FirstName = "[Seleccione el cliente..]",
            });
            return customers.OrderBy(d => d.FirstName).ThenBy( d => d.LastName).ToList();
        }

        public static List<Impuesto> GetImpuestos(int companyId)
        {
            var impuestos = db.Impuestoes.Where(x => x.CompanyID == companyId).ToList();
            impuestos.Add(new Impuesto
            {
                ImpuestoID = 0,
                Description = "[Seleccione el impuesto..]",
            });
            return impuestos.OrderBy(d => d.Description).ToList();
        }
    }
}
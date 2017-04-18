using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;

namespace Ecommerce.Clases
{
    public class CustomerResponse
    {
        public int CustomerID { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int DepartmentID { get; set; }

        public int CityID { get; set; }

        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public Department Department { get; set; }

        public City City { get; set; }

        public List<Order> Orders { get; set; }

        public List<CompanyCustomer> CompanyCustomers { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        [Required (ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(50, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [Display(Name = "Department")]
        [Index("Department_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<City> Cities  { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<User> Users { get; set; }
        
        public virtual ICollection<Warehouse> Warehouses { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
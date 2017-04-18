using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(50, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [Display(Name = "Company")]
        [Index("Company_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(20, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(100, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }
        [NotMapped]
        public HttpPostedFileBase LogoFile { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        public int CityID { get; set; }

       


        public virtual Department Departments { get; set; }

        public virtual City Cities { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Impuesto> Impuestos { get; set; }

        public virtual ICollection<Product> Productos { get; set; }

        public virtual ICollection<Warehouse> Warehouses { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<CompanyCustomer> CompanyCustomers { get; set; }
    }
}
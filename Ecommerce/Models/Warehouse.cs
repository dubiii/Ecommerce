using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Index("Warehouse_Name_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(50, ErrorMessage = "el campo {0} excede los 100 caracteres")]
        [Display(Name = "Nombre Bodega")]
        [Index("Warehouse_Name_Index",2, IsUnique = true)]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(20, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(100, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        public virtual Department Departments { get; set; }

        public virtual City Cities { get; set; }

        public virtual Company Companies { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
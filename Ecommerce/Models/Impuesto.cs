using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Impuesto
    {
        [Key]
        public int ImpuestoID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(50, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [Display(Name = "Impuesto")]
        [Index("Impuesto_Name_Index", 2, IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Range(0.00, 1.00, ErrorMessage = "debe seleccionar un {0} entre {1} y {2}")]
        public double Rate { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Company")]
        [Index("Impuesto_Name_Index", 1, IsUnique = true)]
        public int CompanyID { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Productos { get; set; }
    }
}
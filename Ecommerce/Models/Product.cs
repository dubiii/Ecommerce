using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Company")]
        [Index("Producto_Name_Index", 1, IsUnique = true)]
        [Index("Producto_Name_BarCode_Index", 1, IsUnique = true)]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(50, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [Display(Name = "Producto")]
        [Index("Producto_Name_Index", 2, IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(20, ErrorMessage = "el campo {0} excede los 20 caracteres")]
        [Display(Name = "Bar Code")]
        [Index("Producto_Name_BarCode_Index", 2, IsUnique = true)]
        public string BarCode { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "debe seleccionar un {0}")]
        [Display(Name = "Categoria")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0.00, double.MaxValue, ErrorMessage = "debe seleccionar un {0} entre {1} y {2}")]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "debe seleccionar un {0}")]
        [Display(Name = "Impuesto")]
        public int ImpuestoID { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        //public double Stock { get { return Inventories.Sum(i => i.Stock); } }


        public virtual Company Company { get; set; }
        public virtual Category Category { get; set; }
        public virtual Impuesto Impuesto { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<OrderDetailTemp> OrderDetailTemps { get; set; }
    }
}
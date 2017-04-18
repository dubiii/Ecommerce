using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Ecommerce.Controllers;

namespace Ecommerce.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public virtual Warehouse Warehouses { get; set; }

        public virtual Product Products { get; set; }

    }
}
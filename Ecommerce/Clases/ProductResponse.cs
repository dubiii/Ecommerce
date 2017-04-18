using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;

namespace Ecommerce.Clases
{
    public class ProductResponse
    {
        public int ProductID { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public object ImageFile { get; set; }
        public int ImpuestoID { get; set; }
        public string Remarks { get; set; }
        public Company Company { get; set; }
        public Category Category { get; set; }
        public Impuesto Impuesto { get; set; }
        public Inventory Inventory { get; set; }
        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(256, ErrorMessage = "el campo {0} excede los 100 caracteres")]
        [Display(Name = "E-Mail")]
        [Index("User_Name_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(50, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(50, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(20, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [MaxLength(100, ErrorMessage = "el campo {0} excede los 50 caracteres")]
        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido ")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }


        public virtual Department Departments { get; set; }

        public virtual City Cities { get; set; }

        public virtual Company Companies { get; set; }
    }
}
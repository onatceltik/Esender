using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Esender.Models
{
    public class EmailModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, MinimumLength = 2,
        ErrorMessage = "First name should be minimum 2 characters and a maximum of 50 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Last name should be minimum 2 characters and a maximum of 50 characters")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail Address")]
        [Required(ErrorMessage = "{0} is required!")]
        public string Email { get; set; }
    }
}

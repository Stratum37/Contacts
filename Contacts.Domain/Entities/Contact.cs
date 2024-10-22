using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Domain.Entities
{
    public class Contact
    {
        [Key]
        [ValidateNever]
        public UInt64 Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string ContactFirstName { get; set; } = "";
        [Required]
        [Display(Name = "Last Name")]
        public string ContactLastName { get; set; } = "";

        public string Description { get; set; } = "";
        public string ContactEmail { get; set; } = "";
        public string ContactPhoneNumber { get; set; } = "";
        public string PhotoURL { get; set; } = @"\images\avatar.jpg";

        public DateTime RegDate { get; set; }
        public DateTime ModifiedDT { get; set; }
        public string AuthorName { get; set; } = "";
    }
}

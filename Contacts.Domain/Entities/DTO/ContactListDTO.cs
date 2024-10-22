using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Domain.Entities.DTO
{
    public class ContactListDTO
    {
        [ValidateNever]
        public ulong ContactId { get; set; }
        [MaxLength(127)]
        [DisplayName("First Name")]
        public string ContactFirstName { get; set; } = "";
        [MaxLength(127)]
        [DisplayName("Last Name")]
        public string ContactLastName { get; set; } = "";
        [DisplayName("Email")]
        public string ContactEmail { get; set; } = "";
        [DisplayName("Phone Number")]
        public string ContactPhoneNumber { get; set; }
        [ValidateNever]
        public string PhotoURL { get; set; } = "no photo";
        public string Description { get; set; } = "not present";
    }
}

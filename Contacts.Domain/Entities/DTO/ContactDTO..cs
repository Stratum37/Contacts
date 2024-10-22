using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Domain.Entities.DTO
{
    /// <summary>
    /// Model/DTO for trasferring data. Used avoid unnecesary disclosure of internal data structure. 
    /// Also simplifies data structure (in purpose of demo I use three different DB tables to store contact data). DTO hides DB structure
    /// 
    /// DataAnnotation is used to show how some limitations can be applied to data app operates with.
    /// As this project is a demo, only two types of data are used: string and int/Uint64. 
    /// </summary>
    public class ContactDTO
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
        [ValidateNever]
        public string RegDate { get; set; } = "";
        [ValidateNever]
        public string ModifiedDT { get; set; } = "";

        [MaxLength(10)]
        public string PostalCode { get; set; } = "no data";
        [MaxLength(255)]
        public string Country { get; set; } = "no data";
        [MaxLength(255)]
        public string City { get; set; } = "no data";
        [MaxLength(255)]
        public string Street { get; set; } = "no data";
        [Range(0, 1000)]
        public int BuildingNumber { get; set; } = 0;
        [Range(0, 1000)]
        public int Entrance { get; set; } = 0;
        [Range(0, 1000)]
        public int Floor { get; set; } = 0;
        [Range(0, 1000)]
        public int Flat { get; set; } = 0;
        [MaxLength(255)]
        public string AddressDetails { get; set; } = "no data";

        [MaxLength(255)]
        public string MainEmail { get; set; } = "no data";
        [MaxLength(255)]
        public string WhatsAppName { get; set; } = "no data";
        [MaxLength(255)]
        public string WhatsAppNumber { get; set; } = "no data";
        [MaxLength(255)]
        public string TelegramName { get; set; } = "no data";
        [MaxLength(255)]
        public string TelegramNick { get; set; } = "no data";
        [MaxLength(255)]
        public string TelegramPhoneNumber { get; set; } = "no data";
    }
}

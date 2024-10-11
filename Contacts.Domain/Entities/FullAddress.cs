using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Domain.Entities
{
    public class FullAddress
    {
        [Key]
        [ValidateNever]
        public UInt64 Id { get; set; }
        
        public UInt64 ContactId { get; set; }
        [ForeignKey("ContactId")]
        [ValidateNever]
        public Contact Contact { get; set; }

        public string PostalCode { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public int BuildingNumber { get; set; } = 0;
        public int Entrance { get; set; } = 0;
        public int Floor { get; set; } = 0;
        public int Flat { get; set; } = 0;
        public string Details { get; set; } = "";

        public bool isAuthNeeded { get; set; } = true;  // If enabled then Requestor must be authenticated to have this entity visible in Web or API
        // currently this field isn't used. Assuming that only admin can access contact list via Web. All the other users consume API only

        public bool isVisibleAPI { get; set; } = true; // If enabled then Requestor can get this entity via API request or in search
        public bool isVisibleWeb { get; set; } = true; // If enabled then Requestor can see this entity in Web
    }
}

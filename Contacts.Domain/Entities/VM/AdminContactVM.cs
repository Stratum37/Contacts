using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Domain.Entities.VM
{
    public class AdminContactVM
    {
        [ValidateNever]
        public UInt64 ContactId { get; set; }
        public string ContactFirstName { get; set; } = "";
        public string ContactLastName { get; set; } = "";

        public string Description { get; set; } = "";
        public string ContactEmail { get; set; } = "";
        public string ContactPhoneNumber { get; set; } = "";
        public string PhotoURL { get; set; } = @"\images\avatar.jpg";
        [ValidateNever]
        public DateTime RegDate { get; set; }
        [ValidateNever]
        public DateTime ModifiedDT { get; set; }

        public bool isAdressVisible;
        public bool isMessengerVisible;
        [ValidateNever]
        public VMAddress VMAddress { get; set; }
        [ValidateNever]
        public VMMessenger VMMessenger { get; set; }
    }
    public class VMAddress
    {
        public string PostalCode { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public int BuildingNumber { get; set; } = 0;
        public int Entrance { get; set; } = 0;
        public int Floor { get; set; } = 0;
        public int Flat { get; set; } = 0;
        public string Details { get; set; } = "";
    }

    public class VMMessenger
    {
        public string? MainEmail { get; set; }
        public string? WhatsAppName { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? TelegramName { get; set; }
        public string? TelegramNick { get; set; }
        public string? TelegramPhoneNumber { get; set; }
    }
}

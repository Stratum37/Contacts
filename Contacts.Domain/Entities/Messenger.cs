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
    public class Messenger
    {
        [Key]
        [ValidateNever]
        public UInt64 Id { get; set; }
        [ValidateNever]
        public UInt64 ContactId { get; set; }
        [ForeignKey("ContactId")]
        [ValidateNever]
        public Contact Contact { get; set; }

        public string? MainEmail { get; set; }
        public string? WhatsAppName { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? TelegramName { get; set; }
        public string? TelegramNick { get; set; }
        public string? TelegramPhoneNumber { get; set; }

        public bool isAuthNeeded { get; set; } = true; // User must be authenticated to have this filed accessible
        public bool isVisibleAPI { get; set; } = false;
        public bool isVisibleWeb { get; set; } = false;
    }
}

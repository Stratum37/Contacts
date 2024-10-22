using Contacts.Domain.Entities.DTO;
using Contacts.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Contacts.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Contacts.Application.BL
{
    public class Logic : ILogic
    {
        private readonly ApplicationDbContext _db;
        public Logic(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ContactListDTO> List()
        {
            IEnumerable<ContactListDTO> ContactList = _db.Contacts.Select(c => new ContactListDTO()
            {
                ContactId = c.Id,
                ContactFirstName = c.ContactFirstName,
                ContactLastName = c.ContactLastName,
                ContactEmail = c.ContactEmail,
                ContactPhoneNumber = c.ContactPhoneNumber,
                PhotoURL = c.PhotoURL,
                Description = c.Description
            });

            return ContactList;
        }

        public IEnumerable<ContactListDTO> Search(ListSearchDTO entity)
        {
            throw new NotImplementedException();
        }

        public ulong Add(ContactDTO contactDTO)
        {
            Contact ContactToAdd = new Contact();
            Messenger MessengerToAdd = new Messenger();
            FullAddress AddressToAdd = new FullAddress();

            ContactToAdd.ContactFirstName = contactDTO.ContactFirstName;
            ContactToAdd.ContactLastName = contactDTO.ContactLastName;
            ContactToAdd.ContactPhoneNumber = contactDTO.ContactPhoneNumber;
            ContactToAdd.ContactEmail = contactDTO.ContactEmail;
            ContactToAdd.Description = contactDTO.Description;
            ContactToAdd.RegDate = DateTime.Now;
            ContactToAdd.ModifiedDT = DateTime.Now;
            _db.Add(ContactToAdd);
            _db.SaveChanges();
            contactDTO.ContactId = ContactToAdd.Id;

            MessengerToAdd.ContactId = contactDTO.ContactId;
            MessengerToAdd.MainEmail = contactDTO.MainEmail;
            MessengerToAdd.WhatsAppName = contactDTO.WhatsAppName;
            MessengerToAdd.WhatsAppNumber = contactDTO.WhatsAppNumber;
            MessengerToAdd.TelegramName = contactDTO.TelegramName;
            MessengerToAdd.TelegramNick = contactDTO.TelegramNick;
            MessengerToAdd.TelegramPhoneNumber = contactDTO.TelegramPhoneNumber;

            AddressToAdd.ContactId = contactDTO.ContactId;
            AddressToAdd.PostalCode = contactDTO.PostalCode;
            AddressToAdd.Country = contactDTO.Country;
            AddressToAdd.City = contactDTO.City;
            AddressToAdd.Street = contactDTO.Street;
            AddressToAdd.BuildingNumber = contactDTO.BuildingNumber;
            AddressToAdd.Entrance = contactDTO.Entrance;
            AddressToAdd.Floor = contactDTO.Floor;
            AddressToAdd.Flat = contactDTO.Flat;

            _db.Add(MessengerToAdd);
            _db.Add(AddressToAdd);
            _db.SaveChanges();

            return contactDTO.ContactId;
        }

        public uint Delete(ulong Id)
        {
            var ContactToRemove = _db.Contacts.FirstOrDefault(c => c.Id == Id);
            if (ContactToRemove == null)
            {
                return 1;
            }
            _db.Contacts.Remove(ContactToRemove);
            _db.SaveChanges();

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns>ContactDTO.Id = 0 ==> no record found</returns>
        public ContactDTO Get(UInt64 ContactId)
        {
            ContactDTO contactDTO = new ContactDTO();
            var ContactDb = _db.Contacts.AsNoTracking().FirstOrDefault(c => c.Id == ContactId);
            if (ContactDb == null)
            {
                contactDTO.ContactId = 0;
            }
            else
            {
                contactDTO.ContactId = ContactDb.Id;
                contactDTO.ContactFirstName = ContactDb.ContactFirstName;
                contactDTO.ContactLastName = ContactDb.ContactLastName;
                contactDTO.ContactEmail = ContactDb.ContactEmail;
                contactDTO.ContactPhoneNumber = ContactDb.ContactPhoneNumber;
                contactDTO.Description = ContactDb.Description;
                contactDTO.PhotoURL = ContactDb.PhotoURL;
                contactDTO.RegDate = ContactDb.RegDate.ToString("g");
                contactDTO.ModifiedDT = ContactDb.ModifiedDT.ToString("g");

                var AddressDb = _db.FullAddress.AsNoTracking().FirstOrDefault(a => a.ContactId == ContactId);
                var MessengerDb = _db.Messenger.AsNoTracking().FirstOrDefault(m => m.ContactId == ContactId);
                if(AddressDb != null && AddressDb.isVisibleAPI)
                {
                    contactDTO.PostalCode = AddressDb.PostalCode;
                    contactDTO.Country = AddressDb.Country;
                    contactDTO.City = AddressDb.City;
                    contactDTO.Street = AddressDb.Street;
                    contactDTO.BuildingNumber = AddressDb.BuildingNumber;
                    contactDTO.Entrance = AddressDb.Entrance;
                    contactDTO.Floor = AddressDb.Floor;
                    contactDTO.Flat = AddressDb.Flat;
                    contactDTO.AddressDetails = AddressDb.Details;
                }
                if(MessengerDb != null && MessengerDb.isVisibleAPI)
                {
                    contactDTO.MainEmail = MessengerDb.MainEmail;
                    contactDTO.WhatsAppName = MessengerDb.WhatsAppName;
                    contactDTO.WhatsAppNumber = MessengerDb.WhatsAppNumber;
                    contactDTO.TelegramName = MessengerDb.TelegramName;
                    contactDTO.TelegramNick = MessengerDb.TelegramNick;
                    contactDTO.TelegramPhoneNumber = MessengerDb.TelegramPhoneNumber;
                }
            }
            return contactDTO;
        }

        public uint Update(ContactDTO contactDTO)
        {
            var ContactDb = _db.Contacts.FirstOrDefault(c => c.Id == contactDTO.ContactId);
            var MessengerDb = _db.Messenger.FirstOrDefault(m => m.ContactId == contactDTO.ContactId);
            var AddressDb = _db.FullAddress.FirstOrDefault(fa => fa.ContactId == contactDTO.ContactId);
            if (ContactDb == null || MessengerDb == null || AddressDb == null)
            {
                return 1;
            }

            bool isUpdated = false;
            bool isAddressUpdated = false;
            bool isMessengerUpdated = false;
            if (contactDTO.ContactFirstName != ContactDb.ContactFirstName && !contactDTO.ContactFirstName.IsNullOrEmpty())
            {
                ContactDb.ContactFirstName = contactDTO.ContactFirstName;
                isUpdated = true;
            }
            if (contactDTO.ContactLastName != ContactDb.ContactLastName && !contactDTO.ContactLastName.IsNullOrEmpty())
            {
                ContactDb.ContactLastName = contactDTO.ContactLastName;
                isUpdated = true;
            }
            if (contactDTO.ContactEmail != ContactDb.ContactEmail && !contactDTO.ContactEmail.IsNullOrEmpty())
            {
                ContactDb.ContactEmail = contactDTO.ContactEmail;
                isUpdated = true;
            }
            if (contactDTO.ContactPhoneNumber != ContactDb.ContactPhoneNumber && !contactDTO.ContactPhoneNumber.IsNullOrEmpty())
            {
                ContactDb.ContactPhoneNumber = contactDTO.ContactPhoneNumber;
                isUpdated = true;
            }
            if (contactDTO.Description != ContactDb.Description && !contactDTO.Description.IsNullOrEmpty())
            {
                ContactDb.Description = contactDTO.Description;
                isUpdated = true;
            }

            if (AddressDb.PostalCode != contactDTO.PostalCode)
            {
                AddressDb.PostalCode = contactDTO.PostalCode;
                isAddressUpdated = true;
            }
            if (AddressDb.Country != contactDTO.Country)
            {
                AddressDb.Country = contactDTO.Country;
                isAddressUpdated = true;
            }
            if (AddressDb.City != contactDTO.City)
            {
                AddressDb.City = contactDTO.City;
                isAddressUpdated = true;
            }
            if (AddressDb.Street != contactDTO.Street)
            {
                AddressDb.Street = contactDTO.Street;
                isAddressUpdated = true;
            }
            if (AddressDb.BuildingNumber != contactDTO.BuildingNumber && contactDTO.BuildingNumber > 0)
            {
                AddressDb.BuildingNumber = contactDTO.BuildingNumber;
                isAddressUpdated = true;
            }
            if (AddressDb.Entrance != contactDTO.Entrance && contactDTO.Entrance > 0)
            {
                AddressDb.Entrance = contactDTO.Entrance;
                isAddressUpdated = true;
            }
            if (AddressDb.Floor != contactDTO.Floor && contactDTO.Floor > 0)
            {
                AddressDb.Floor = contactDTO.Floor;
                isAddressUpdated = true;
            }
            if (AddressDb.Flat != contactDTO.Flat && contactDTO.Flat > 0)
            {
                AddressDb.Flat = contactDTO.Flat;
                isAddressUpdated = true;
            }
            if (AddressDb.Details != contactDTO.AddressDetails)
            {
                AddressDb.Details = contactDTO.AddressDetails;
                isAddressUpdated = true;
            }

            if (isAddressUpdated)
            {
                isUpdated = true;
                _db.Update(AddressDb);
            }
            if (isMessengerUpdated)
            {
                isUpdated = true;
                _db.Update(MessengerDb);
            }
            if (isUpdated)
            {
                ContactDb.ModifiedDT = DateTime.Now;
                _db.Update(ContactDb);
                _db.SaveChanges();
            }

            return 0;
        }
    }
}

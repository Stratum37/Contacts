using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contacts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Contacts.Domain.Entities.VM;
using Contacts.Domain.Entities;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace Contacts.Web.Controllers
{
    /// <summary>
    /// Attribute [ApiController] allows validation to be done automatically before sending data to endpoint. 
    /// But I also check ModelState to be sure that incoming data are comply with Model/DTO 
    /// For test purposes Swagger can be used.
    /// 
    /// In methods number of checks applied to ensure that incoming data are correct. In real app/web additional checks should be done:
    /// frontend/mobile app/ desktop app --> using restricted input controls and performing affitional checks that correct and allowed data were input
    /// Checking incoming data before storing is necessary. We can't be 100% sure that source provide correct data (checks might be mallfunctioned or intentionally incorrect).
    /// 
    /// </summary>


    [ApiController]
    [Route("/api/ContactAPI")]
    public class APIController : ControllerBase
    {
        // Dependency Injection. Adding DB related methods support 
        private readonly ApplicationDbContext _db;
        public APIController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Endpoints. Every endpoint contains declaration of possible responses
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Contact>> ContactList()
        {
            var ContactList = _db.Contacts.ToList();
            if (ContactList.Count == 0)
            {
                return NotFound();
            }
            return Ok(ContactList);
        }

        [HttpGet("GetOne")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ContactDTO> Contact(UInt64 Id)
        {
            if (Id <= 0)
            { return BadRequest(); }

            ContactDTO ContactDTO = new ContactDTO();
            var ContactDb = _db.Contacts.AsNoTracking().FirstOrDefault(c => c.Id == Id);
            var MessengerDb = _db.Messenger.AsNoTracking().FirstOrDefault(c => c.ContactId == Id);
            var FullAddressDb = _db.FullAddress.AsNoTracking().FirstOrDefault(c => c.ContactId == Id);
            if (ContactDb == null || MessengerDb == null || FullAddressDb == null)
            {
                return NotFound();
            }

            ContactDTO.ContactId = Id;
            ContactDTO.ContactFirstName = ContactDb.ContactFirstName;
            ContactDTO.ContactLastName = ContactDb.ContactLastName;
            ContactDTO.ContactPhoneNumber = ContactDb.ContactPhoneNumber;
            ContactDTO.ContactEmail = ContactDb.ContactEmail;
            ContactDTO.Description = ContactDb.Description;
            ContactDTO.PhotoURL = ContactDb.PhotoURL;
            ContactDTO.RegDate = ContactDb.RegDate.ToString();
            ContactDTO.ModifiedDT = ContactDb.ModifiedDT.ToString();

            if (MessengerDb.isVisibleAPI)
            {
                ContactDTO.MainEmail = MessengerDb.MainEmail;
                ContactDTO.TelegramName = MessengerDb.TelegramName;
                ContactDTO.TelegramPhoneNumber = MessengerDb.TelegramPhoneNumber;
                ContactDTO.TelegramNick = MessengerDb.TelegramNick;
                ContactDTO.WhatsAppName = MessengerDb.WhatsAppName;
                ContactDTO.WhatsAppNumber = MessengerDb.WhatsAppNumber;
            }
            if (FullAddressDb.isVisibleAPI)
            {
                ContactDTO.PostalCode = FullAddressDb.PostalCode;
                ContactDTO.Country = FullAddressDb.Country;
                ContactDTO.City = FullAddressDb.City;
                ContactDTO.Street = FullAddressDb.Street;
                ContactDTO.BuildingNumber = FullAddressDb.BuildingNumber;
                ContactDTO.Entrance = FullAddressDb.Entrance;
                ContactDTO.Floor = FullAddressDb.Floor;
                ContactDTO.Flat = FullAddressDb.Flat;
                ContactDTO.AddressDetails = FullAddressDb.Details;
            }

            return Ok(ContactDTO);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ContactDTO> Create([FromBody] ContactDTO contactDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (contactDTO == null)
            {
                return BadRequest(contactDTO);
            }
            if (contactDTO.ContactId > 0)
            {
                return BadRequest(contactDTO);
            }

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
            if (ContactToAdd.Id == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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

            return CreatedAtRoute("Contact", new { Id = ContactToAdd.Id }, contactDTO);
        }

        [HttpDelete("Delete", Name = "DeleteContact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteContact(UInt64 Id)
        {
            if (Id <= 0)
            {
                return BadRequest();
            }

            var ContactToRemove = _db.Contacts.FirstOrDefault(c => c.Id == Id);
            if (ContactToRemove == null)
            {
                return NotFound();
            }

            _db.Contacts.Remove(ContactToRemove);
            _db.SaveChanges();

            return NoContent();

        }

        /// <summary>
        /// This method uses received ContactDTO object as a source of updated data. 
        /// Several checks performed before update:
        /// * if(!ModelState.IsValid) --> ModelState is incorrect. Return state details in response
        /// * if(ContactId <= 0) --> incorrect or inconsistent data from source. Return;
        /// ContactId > 0 and Contact record not found in DB - probably not correct Id, or other error, returning NotFound;
        /// Method updates only changed fields so I didn't implement HttpPatch method.
        /// </summary>
        /// <param name="contactDTO"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateContact([FromBody] ContactDTO contactDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (contactDTO.ContactId <= 0)
            {
                return BadRequest("Incorrect Id");
            }
            var ContactDb = _db.Contacts.FirstOrDefault(c => c.Id == contactDTO.ContactId);
            if (ContactDb == null)
            {
                return NotFound();
            }
            var MessengerDb = _db.Messenger.FirstOrDefault(m => m.ContactId == contactDTO.ContactId);
            var AddressDb = _db.FullAddress.FirstOrDefault(fa => fa.ContactId == contactDTO.ContactId);
            if (MessengerDb == null || AddressDb == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
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

            return NoContent();
        }


    }
}

using Contacts.Domain.Entities;
using Contacts.Domain.Entities.VM;
using Contacts.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;

namespace Contacts.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult ContactList()
        {
            var ContactList = _db.Contacts.ToList();
            if (ContactList.Count == 0)
            {
                TempData["warning"] = "No contacts found";
            }
            return View(ContactList);
        }

        public IActionResult Contact(UInt64? ContactId)
        {
            AdminContactVM admincontactVM = new AdminContactVM()
            {
                VMAddress = new(),
                VMMessenger = new()
            };

            //Existing contact, loading data
            if (ContactId > 0)
            {
                var ContactDb = _db.Contacts.AsNoTracking().FirstOrDefault(c => c.Id == ContactId);
                if (ContactDb != null)
                {
                    var MessengerDb = _db.Messenger.AsNoTracking().FirstOrDefault(c => c.ContactId == ContactId);
                    var FullAddressDb = _db.FullAddress.AsNoTracking().FirstOrDefault(c => c.ContactId == ContactId);

                    admincontactVM.ContactId = ContactDb.Id;
                    admincontactVM.ContactFirstName = ContactDb.ContactFirstName;
                    admincontactVM.ContactLastName = ContactDb.ContactLastName;
                    admincontactVM.ContactPhoneNumber = ContactDb.ContactPhoneNumber;
                    admincontactVM.ContactEmail = ContactDb.ContactEmail;
                    admincontactVM.Description = ContactDb.Description;
                    admincontactVM.PhotoURL = ContactDb.PhotoURL;
                    admincontactVM.RegDate = ContactDb.RegDate;
                    admincontactVM.ModifiedDT = ContactDb.ModifiedDT;

                    if (MessengerDb != null)
                    {
                        admincontactVM.VMMessenger.MainEmail = MessengerDb.MainEmail;
                        admincontactVM.VMMessenger.TelegramName = MessengerDb.TelegramName;
                        admincontactVM.VMMessenger.TelegramPhoneNumber = MessengerDb.TelegramPhoneNumber;
                        admincontactVM.VMMessenger.TelegramNick = MessengerDb.TelegramNick;
                        admincontactVM.VMMessenger.WhatsAppName = MessengerDb.WhatsAppName;
                        admincontactVM.VMMessenger.WhatsAppNumber = MessengerDb.WhatsAppNumber;
                        admincontactVM.isMessengerVisible = MessengerDb.isVisibleAPI;
                    }
                    else
                    {
                        //couldn't load Messenger record. Handle possible error
                        //admincontactVM.VMMessenger.
                    }
                    if (FullAddressDb != null)
                    {
                        admincontactVM.VMAddress.PostalCode = FullAddressDb.PostalCode;
                        admincontactVM.VMAddress.Country = FullAddressDb.Country;
                        admincontactVM.VMAddress.City = FullAddressDb.City;
                        admincontactVM.VMAddress.Street = FullAddressDb.Street;
                        admincontactVM.VMAddress.BuildingNumber = FullAddressDb.BuildingNumber;
                        admincontactVM.VMAddress.Entrance = FullAddressDb.Entrance;
                        admincontactVM.VMAddress.Floor = FullAddressDb.Floor;
                        admincontactVM.VMAddress.Flat = FullAddressDb.Flat;
                        admincontactVM.VMAddress.Details = FullAddressDb.Details;
                        admincontactVM.isAdressVisible = FullAddressDb.isVisibleAPI;
                    }
                    else
                    {
                        //couldn't load FullAddress record. Handle possible error
                    }
                }
                else
                {
                    // Contact wasn't loaded or found. Handle this situation
                }
            }
            else
            {
                // new contact. Since no additional actions required when creating new card, else section is empty
            }
            return View(admincontactVM);
        }

        [HttpPost]
        public IActionResult Contact(AdminContactVM adminContactVM, bool isAdressVisible, bool isMessengerVisible)
        {
            if (ModelState.IsValid)
            {
                if (adminContactVM.ContactId > 0) // If Id > 0, then updating existing records
                {
                    bool isUpdated = false;
                    var ContactDb = _db.Contacts.FirstOrDefault(c => c.Id == adminContactVM.ContactId);
                    if (ContactDb != null)  // Checking if record found and loaded.
                    {
                        if (ContactDb.ContactEmail != adminContactVM.ContactEmail)
                        { 
                            ContactDb.ContactEmail = adminContactVM.ContactEmail; 
                            isUpdated = true;
                        }
                        if (ContactDb.ContactFirstName != adminContactVM.ContactFirstName)
                        { 
                            ContactDb.ContactFirstName = adminContactVM.ContactFirstName;
                            isUpdated = true;
                        }
                        if (ContactDb.ContactLastName != adminContactVM.ContactLastName)
                        { 
                            ContactDb.ContactLastName = adminContactVM.ContactLastName; 
                            isUpdated = true;
                        }
                        if (ContactDb.Description != adminContactVM.Description)
                        { 
                            ContactDb.Description = adminContactVM.Description; 
                            isUpdated = true;
                        }
                        if (ContactDb.ContactPhoneNumber != adminContactVM.ContactPhoneNumber)
                        { 
                            ContactDb.ContactPhoneNumber = adminContactVM.ContactPhoneNumber; 
                            isUpdated = true;
                        }

                        var ContactAddressDb = _db.FullAddress.FirstOrDefault(ad => ad.ContactId == ContactDb.Id);
                        if (ContactAddressDb != null)
                        {
                            bool isAddressUpdated = false;
                            if (ContactAddressDb.isVisibleAPI != isAdressVisible)
                            { 
                                ContactAddressDb.isVisibleAPI = isAdressVisible;
                                isAddressUpdated = true;
                            }
                            if (ContactAddressDb.PostalCode != adminContactVM.VMAddress.PostalCode)
                            { 
                                ContactAddressDb.PostalCode = adminContactVM.VMAddress.PostalCode;
                                isAddressUpdated = true;
                            }
                            if (ContactAddressDb.Country != adminContactVM.VMAddress.Country)
                            { 
                                ContactAddressDb.Country = adminContactVM.VMAddress.Country;
                                isAddressUpdated = true;
                            }
                            if (ContactAddressDb.City != adminContactVM.VMAddress.City)
                            { 
                                ContactAddressDb.City = adminContactVM.VMAddress.City;
                                isAddressUpdated = true;
                            }
                            if (ContactAddressDb.Street != adminContactVM.VMAddress.Street)
                            { 
                                ContactAddressDb.Street = adminContactVM.VMAddress.Street;
                                isAddressUpdated = true;
                            }
                            if (ContactAddressDb.BuildingNumber != adminContactVM.VMAddress.BuildingNumber)
                            { 
                                ContactAddressDb.BuildingNumber = adminContactVM.VMAddress.BuildingNumber;
                                isAddressUpdated = true;
                            }
                            if (ContactAddressDb.Flat != adminContactVM.VMAddress.Flat)
                            {
                                ContactAddressDb.Flat = adminContactVM.VMAddress.Flat;
                                isAddressUpdated = true;
                            }
                            if (ContactAddressDb.Details != adminContactVM.VMAddress.Details)
                            { 
                                ContactAddressDb.Details = adminContactVM.VMAddress.Details;
                                isAddressUpdated = true;
                            }
                            if (isAddressUpdated)
                            {
                                _db.Update(ContactAddressDb);
                                isUpdated = true;
                            }
                        }
                        else
                        {
                            // error occured. Need to handle
                            FullAddress ContactAddressToAdd = new()
                            {
                                ContactId = ContactDb.Id,
                                isVisibleAPI = isAdressVisible,
                                PostalCode = adminContactVM.VMAddress.PostalCode,
                                Country = adminContactVM.VMAddress.Country,
                                City = adminContactVM.VMAddress.City,
                                Street = adminContactVM.VMAddress.Street,
                                BuildingNumber = adminContactVM.VMAddress.BuildingNumber,
                                Flat = adminContactVM.VMAddress.Flat,
                                Details = adminContactVM.VMAddress.Details,
                            };
                            _db.Add(ContactAddressToAdd);
                            isUpdated = true;
                        }

                        var ContactMessengerDb = _db.Messenger.AsNoTracking().FirstOrDefault(m => m.ContactId == ContactDb.Id);
                        if (ContactMessengerDb != null)
                        {
                            bool isMessengerUpdated = false;
                            if (ContactMessengerDb.isVisibleAPI != isMessengerVisible)
                            { 
                                ContactMessengerDb.isVisibleAPI = isMessengerVisible;
                                isMessengerUpdated = true;
                            }

                            if (ContactMessengerDb.MainEmail != adminContactVM.VMMessenger.MainEmail)
                            { 
                                ContactMessengerDb.MainEmail = adminContactVM.VMMessenger.MainEmail;
                                isMessengerUpdated = true;
                            }
                            if (ContactMessengerDb.WhatsAppName != adminContactVM.VMMessenger.WhatsAppName)
                            { 
                                ContactMessengerDb.WhatsAppName = adminContactVM.VMMessenger.WhatsAppName;
                                isMessengerUpdated = true;
                            }
                            if (ContactMessengerDb.WhatsAppNumber != adminContactVM.VMMessenger.WhatsAppNumber)
                            { 
                                ContactMessengerDb.WhatsAppNumber = adminContactVM.VMMessenger.WhatsAppNumber;
                                isMessengerUpdated = true;
                            }
                            if (ContactMessengerDb.TelegramName != adminContactVM.VMMessenger.TelegramName)
                            { 
                                ContactMessengerDb.TelegramName = adminContactVM.VMMessenger.TelegramName;
                                isMessengerUpdated = true;
                            }
                            if (ContactMessengerDb.TelegramNick != adminContactVM.VMMessenger.TelegramNick)
                            { 
                                ContactMessengerDb.TelegramNick = adminContactVM.VMMessenger.TelegramNick;
                                isMessengerUpdated = true;
                            }
                            if (ContactMessengerDb.TelegramPhoneNumber != adminContactVM.VMMessenger.TelegramPhoneNumber)
                            { 
                                ContactMessengerDb.TelegramPhoneNumber = adminContactVM.VMMessenger.TelegramPhoneNumber;
                                isMessengerUpdated = true;
                            }
                            if (isMessengerUpdated)
                            {
                                _db.Update(ContactMessengerDb);
                                isUpdated = true;
                            }
                        }
                        else
                        {
                            Messenger ContactMessengerToAdd = new()
                            {
                                ContactId = ContactDb.Id,
                                isVisibleAPI = isMessengerVisible,
                                MainEmail = adminContactVM.VMMessenger.MainEmail,
                                WhatsAppName = adminContactVM.VMMessenger.WhatsAppName,
                                WhatsAppNumber = adminContactVM.VMMessenger.WhatsAppNumber,
                                TelegramName = adminContactVM.VMMessenger.TelegramName,
                                TelegramNick = adminContactVM.VMMessenger.TelegramNick,
                                TelegramPhoneNumber = adminContactVM.VMMessenger.TelegramPhoneNumber
                            };
                            _db.Add(ContactMessengerToAdd);
                            isUpdated = true;
                        }

                        if(isUpdated)
                        {
                            ContactDb.ModifiedDT = DateTime.Now;
                            _db.Update(ContactDb);
                        }
                    }
                    else
                    {
                        TempData["error"] = "Не удалось созранить изменения в карточке контакта, не удалось найти карточку в БД";
                        return View(adminContactVM);
                        //couldn't load contact. Handle the situation
                    }
                }
                else // Creating new record 
                {
                    Contact ContactToAdd = new()
                    {
                        ContactEmail = adminContactVM.ContactEmail,
                        ContactFirstName = adminContactVM.ContactFirstName,
                        ContactLastName = adminContactVM.ContactLastName,
                        Description = adminContactVM.Description,
                        ContactPhoneNumber = adminContactVM.ContactPhoneNumber,
                        RegDate = DateTime.Now
                    };
                    _db.Add(ContactToAdd);
                    _db.SaveChanges();
                    adminContactVM.ContactId = ContactToAdd.Id;

                    FullAddress ContactAddressToAdd = new()
                    {
                        ContactId = ContactToAdd.Id,
                        PostalCode = adminContactVM.VMAddress.PostalCode,
                        Country = adminContactVM.VMAddress.Country,
                        City = adminContactVM.VMAddress.City,
                        Street = adminContactVM.VMAddress.Street,
                        BuildingNumber = adminContactVM.VMAddress.BuildingNumber,
                        Entrance = adminContactVM.VMAddress.Entrance,
                        Floor = adminContactVM.VMAddress.Floor,
                        Flat = adminContactVM.VMAddress.Flat,
                        Details = adminContactVM.VMAddress.Details
                    };
                    Messenger ContactMessengerToSave = new Messenger()
                    { 
                        ContactId = ContactToAdd.Id,
                        MainEmail = adminContactVM.VMMessenger.MainEmail,
                        WhatsAppName = adminContactVM.VMMessenger.WhatsAppName,
                        WhatsAppNumber = adminContactVM.VMMessenger.WhatsAppNumber,
                        TelegramName = adminContactVM.VMMessenger.TelegramName,
                        TelegramNick = adminContactVM.VMMessenger.TelegramNick,
                        TelegramPhoneNumber = adminContactVM.VMMessenger.TelegramPhoneNumber
                    };
                    _db.Add(ContactAddressToAdd);
                    _db.Add(ContactMessengerToSave);
                }

                _db.SaveChanges();
            }
            else
            {
                TempData["warning"] = "There are errors in data from form.";
            }
            return RedirectToAction(nameof(Contact), new { ContactId = adminContactVM.ContactId });
        }

        public IActionResult DeleteContact(UInt64 ContactId)
        {
            if (ContactId > 0)
            {
                var ContactDb = _db.Contacts.FirstOrDefault(c => c.Id == ContactId);
                if (ContactDb != null)
                {
                    _db.Remove(ContactDb);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Couldn't delete contact card. Couldn't find it in DB.";
                }
            }
            else
            {
                // ContactId isn't valid
                TempData["warning"] = "Id for contact isn't valid. Try to delete contact once again";
            }

            return RedirectToAction(nameof(ContactList));
        }
    }
}

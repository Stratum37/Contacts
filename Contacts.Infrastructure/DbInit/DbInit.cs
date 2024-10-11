using Contacts.Domain.Entities;
using Contacts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Infrastructure.DbInit
{
    public class DbInit : IDbInit
    {
        private readonly ApplicationDbContext _db;

        public DbInit(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Initialize()
        {

            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            var JackSparrowCallsforHelp = _db.Contacts.AsNoTracking().Where(c => c.ContactFirstName == "Jack" && c.ContactLastName == "Sparrow").Count() == 0;
            if (JackSparrowCallsforHelp)
            {
                Contact contact = new()
                {
                    ContactFirstName = "Jack",
                    ContactLastName = "Sparrow",
                    ContactEmail = "Sparrow@blackpearl",
                    ContactPhoneNumber = "123456",
                    Description = "We get him back and that tricky pirate didn't say thank you!",
                    RegDate = DateTime.UtcNow,
                    ModifiedDT = DateTime.UtcNow
                };
                _db.Contacts.Add(contact);
                _db.SaveChanges();
                Messenger messenger = new()
                {
                    ContactId = contact.Id,
                    MainEmail = "JackSeaWolf@Carribean",
                    TelegramName = "big_feather_of_Carribean",
                    TelegramPhoneNumber = "555-1234",
                    isVisibleAPI = true
                };
                FullAddress address = new()
                {
                    isVisibleAPI = false,
                    ContactId = contact.Id,
                    PostalCode = "112233",
                    Country = "Carribean",
                    City = "Jamaica",
                    Street = "HarbourPathWalk",
                    BuildingNumber = 1,
                    Entrance = 1,
                    Floor = 2,
                    Flat = 4,
                    Details = "Leave burrel of rum in front of the door and puch the lever",
                };

                _db.Add(messenger);
                _db.Add(address);
                _db.SaveChanges();
            }

            var isCreatorContactExists = _db.Contacts.AsNoTracking().FirstOrDefault(c => c.ContactFirstName == "Aleksandr" && c.ContactLastName == "Ilev");
            if (isCreatorContactExists == null)
            {
                Contact contact = new()
                {
                    ContactFirstName = "Aleksandr",
                    ContactLastName = "Ilev",
                    ContactEmail = "synerby@gmail.com",
                    ContactPhoneNumber = "+79639648264",
                    Description = "Wanted to be sure you see my contacts =）",
                    RegDate = DateTime.UtcNow,
                    ModifiedDT = DateTime.UtcNow
                };
                _db.Contacts.Add(contact);
                _db.SaveChanges();
                Messenger messenger = new()
                {
                    isVisibleAPI = true,
                    ContactId = contact.Id,
                    MainEmail = "aleksandr@1984@mail.ru",
                    TelegramName = "A I",
                    TelegramNick = "+79639648264",
                    TelegramPhoneNumber = "+79639648264",
                    WhatsAppNumber = "+79639648264"
                };
                FullAddress address = new()
                {
                    isVisibleAPI = true,
                    ContactId = contact.Id,
                    PostalCode = "",
                    Country = "VietNam",
                    City = "DaNang",
                    Details = "While traveling (me and my wife), staying for 2-3 months in each country we visit. Remote, available more than 3 hours a day.",
                };

                _db.Add(messenger);
                _db.Add(address);
                _db.SaveChanges();
                
            }
            return;
        }
    }
}
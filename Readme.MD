## Description
Provided solution implements demo of layered application to work with contacts via web or API. App stores data in database.
Each contact consists of 3 parts:
- Contact card 
- FullAddress card (has relation to Contact card: FK => Contact.Id. Can be hidden from API requests)
- Messenger card (has relation to Contact card: FK => Contact.Id. Can be hidden from API requests)

For each contact it's possible to adjust if API able to retreive messenger or address data. Restrictions applied to API retreiving requests only. Data editing via API isn't restricted.  
Despite native input validation capabilities, both controllers (for Web and for API) implement simple checks if required data presented. Detailed error data implemented for some errors.  
Web and API allow to get list of existing contacts, view particular card, modify or delete card. Web is intended for testing purpose, based on MVC, has no additional view design or front-end features like ajax

API can used via Swagger or Postman. API endpoint - /api/ContactAPI (https://localhost:7031/api/ContactAPI)
Available methods:  
HTTPGET /api/ContactAPI/GetAll - Get contact list (main cards)  
HTTPPOST /api/ContactAPI/Add - Add contact to DB  
HTTPPUT /api/ContactAPI/Update - Update contact and details. Method check and update only changed fields. That's why I didn;t implement HttpPatch method  
HTTPGET /api/ContactAPI/GetOne/Id - Get particular contact details (Main card - always, Full address details and Messenger details if each allows retreival via API)  
HTTPDELETE] /api/ContactAPI/Delete/Id - Delete contact

## Install:
1) Clone from git or download (https://drive.google.com/file/d/1Si1w6XTNKWStfvM2f2yVcjTx0HtlFK99/view?usp=drive_link) and unpack archive, launch Contacts.sln
2) Adjust DB connection settings. Navigate to file Contacts.Web\appsettings.json and modify block "DefaultConnection" to fit your DB connection parameters
3) Create DB, tables and demo contact:
- automatically:  
set Contacts.Web as a startup project and launch app. At every launch pending migrations will be applied automatically and tables will be seeded with demo contacts if they are missing. To stop seeding you can comment or delete string with method "DbSeed();" from file Contacts.Web\program.cs. 
When Web page is appears you can navigate to "Contact List" link to see sample contact cards. 
- manually:  
3.1) remove folder (Contacts\Contacts.Infrastructure\Migrations\)  
3.2) navigate to Package Manager Console, choose Contacts.Infrastructure as Default Project  
3.3) in CMD (PM>) execute command (omit # ): # add-migration ADDTBLS  
After successfull building migration file will be created and shown for review. Keep it unchanged. ReferentialAction should be kept as Cascade. 
FullAddress and Messenger tables are related to main card so have foreign key. They are useless without main card and will be deleted when main card is deleted. ReferentialAction.Cascade indicates this.  
3.4) in CMD (PM>) execute command: # update-database  
3.5 set Contacts.Web as a startup project and launch app. When web page is loaded you can check Contact List link to see Jack Sparrow contact.  
4) Add couple of additional contacts with different visibility for API (check or uncheck appropriate checkbox for address and messenger details).  
5) Open Swagger. Request each contact you created and check if address and messenger details are presented depending on visibility settings you made via web.

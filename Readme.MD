## Description
Demo project to show webAPI implementation. App data stored in database.
App uses layered architecture to separate layers: API/Presentation <= Logic <= Entities and Infrastructure

Each contact consists of 3 parts: Contact card, FullAddress card and Messenger card. Address and Messenger cards have relation to main card.
For each contact it's possible to adjust if API able to retreive messenger or address data. Restrictions applied to API retreiving requests only. Data editing via API isn't restricted. 
Controllers implement simple checks if required data presented. Detailed error data implemented for some errors.  

API can be used via Swagger or Postman. API endpoint - /api/ContactAPI (https://localhost:7031/api/ContactAPI)
Available methods:  
HTTPGET /api/ContactAPI/GetAll - Get contact list (main cards)  
HTTPPOST /api/ContactAPI/Add - Add contact to DB  
HTTPPUT /api/ContactAPI/Update - Update contact and details. Method check and update only changed fields. That's why I didn;t implement HttpPatch method  
HTTPGET /api/ContactAPI/GetOne/Id - Get particular contact details (Main card - always, Full address details and Messenger details if each allows retreival via API)  
HTTPDELETE] /api/ContactAPI/Delete/Id - Delete contact

## Install:  
1) Clone from git, launch Contacts.sln  
2) Adjust DB connection settings (file Contacts.Web\appsettings.json, modify block "DefaultConnection".  
3) Create DB, tables and demo contact:  
- automatically:  
set Contacts.Web as a startup project and launch app. At every launch pending migrations will be applied automatically and tables will be seeded with demo contacts if they are missing. To stop seeding you can comment or delete string with method "DbSeed();" from file Contacts.Web\program.cs. 
When Web page is appears you can navigate to "Contact List" link to see sample contact cards.  
- manually:  
3.1) navigate to Package Manager Console, choose Contacts.Infrastructure as Default Project  
3.2) in CMD (PM>) execute commands (one line at a time):  
add-migration ADDTBLS  
update-database  
3.3) set Contacts.Web as a startup project and launch app. When web page is loaded you can check Contact List link to see Jack Sparrow contact.  
4) Add couple of additional contacts with different visibility for API (check or uncheck appropriate checkbox for address and messenger details).  
5) Open Swagger. Request each contact you created and check if address and messenger details are presented depending on visibility settings you made via web.

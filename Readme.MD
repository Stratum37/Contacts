# Description
Provided solution implements demo of layered application to work with contacts via web or API. App stores data in database.
Each contact consists of 3 parts:
- Contact card 
- FullAddress card (has relation to Contact card: FK => Contact.Id. Can be hidden from API requests)
- Messenger card (has relation to Contact card: FK => Contact.Id. Can be hidden from API requests)
For each contact it's possible to adjust if API able to retreive messenger or address data. Restrictions applied to API retreiving only. Data editing via API isn't restricted
Despite native input validation capabilities, both controllers (for Web and for API) implement simple checks if required data presented. Detailed error data implemented for some errors.

Web and API allow to get list of existing contacts, view particular card, modify or delete card.
Web is intended for testing purpose, based on MVC, has no additional view design or front-end features like ajax

API can used via Swagger or Postman. API endpoint - /api/ContactAPI (https://localhost:7031/api/ContactAPI)
Available methods:
GET /api/ContactAPI - Get contact list (main cards)
POST /api/ContactAPI - Add contact to DB
PUT /api/ContactAPI - Update contact and details. Method check and update only changed fields. That's why I didn;t implement HttpPatch method
GET /api/ContactAPI/Id - Get particular contact details (Main card - always, Full address details and Messenger details if each allows retreival via API)
DELETE /api/ContactAPI/Id - Delete contact

** DTO:
{
  "contactId": 0,
  "contactFirstName": "string",
  "contactLastName": "string",
  "contactEmail": "string",
  "contactPhoneNumber": "string",
  "photoURL": "string",
  "description": "string",
  "regDate": "string",
  "modifiedDT": "string",
  "postalCode": "string",
  "country": "string",
  "city": "string",
  "street": "string",
  "buildingNumber": 1000,
  "entrance": 1000,
  "floor": 1000,
  "flat": 1000,
  "addressDetails": "string",
  "mainEmail": "string",
  "whatsAppName": "string",
  "whatsAppNumber": "string",
  "telegramName": "string",
  "telegramNick": "string",
  "telegramPhoneNumber": "string"
}


# Install:
** 1) Clone from git or download (https://drive.google.com/file/d/1Si1w6XTNKWStfvM2f2yVcjTx0HtlFK99/view?usp=drive_link) and unpack archive, launch Contacts.sln
** 2) Dependency packages will be installed automatically
[Microsoft.EntityFrameworkCore (8.0.8) 
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.SqlServer.Design
Microsoft.EntityFrameworkCore.Tools]
** 3) adjust initial settings
- Navigate to file Contacts.Web\appsettings.json and modify block "DefaultConnection", adjust settings to suit your DB connection parameters
- Create DB, tables and demo contact. 
-- automatically: set Contacts.Web as a startup project and launch app. At every launch (Contacts.Web) pending migrations will be applied automatically and tables will be seeded with demo contact if it's missing. To stop seeding you can comment or delete string with method "DbSeed();" from file Contacts.Web\program.cs. When Web page is appears you can navigate to Contact List link to see Jack Sparrow contact card. 
-- manually: remove folder (Contacts\Contacts.Infrastructure\Migrations\)
-- navigate to Package Manager Console, choose Contacts.Infrastructure as Default Project
-- in CMD (PM>) execute command (omit # ): # add-migration ADDTBLS
-- after successfull building migration file will be created and shown for review. Keep it unchanged. ReferentialAction should be kept as Cascade. 
FullAddress and Messenger tables are related to main card so have foreign key. They are useless without main card and will be deleted when main card is deleted. ReferentialAction.Cascade indicates this.
-- in CMD (PM>) execute command: # update-database
-- set Contacts.Web as a startup project and launch app. When web page is loaded you can check Contact List link to see Jack Sparrow contact. 
** 4) Add couple of additional contacts with different visibility for API (check or uncheck appropriate checkbox for address and messenger details). 
** 5) switch startup project to Contacts.API and launch app. Request each contact you created and check if address and messenger details are presented depending on visibility settings you made via web.

===========

Инструкции:
1) установка
* Клонируйте из git либо скачайте архив по ссылке (https://drive.google.com/file/d/1Si1w6XTNKWStfvM2f2yVcjTx0HtlFK99/view?usp=drive_link), распакуйте и запустите файл Contacts.sln
* пакеты-зависимости установятся автоматически
[Microsoft.EntityFrameworkCore (8.0.8) 
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.SqlServer.Design
Microsoft.EntityFrameworkCore.Tools]

2) Выполните первичную настройку (в dev-среде)
- в файле Contacts.Web\appsettings.json в блоке "DefaultConnection" поменяйте параметры подключения к СУБД (адрес сервера, имя БД и т.д.)
- Создайте БД, таблицы и демо контакт. Для этого: 
2.1) Примените миграцию к БД (миграция создаёт, меняет и/или удаляет таблицы и их структуру в БД):
- Автоматически - установить Contacts.web в качестве startup-проекта и запустить приложение. При запуске применяются миграции и создаётся демо контакт в БД, если он отсутствует.
Чтобы отключить автосоздание, нужно в файле Contacts.Web\program.cs удалить или закоментировать строку DbSeed();
Файл первичной миграции присутствует в поставке.
* Вручную:
* удалить папку (Contacts\Contacts.Infrastructure\Migrations\)
* перейти в Package Manager Console, в выпадающем меню Default Project: выбрать Contacts.Infrastructure
* в CMD (PM>) набрать и выполнить команду (без # ): # add-migration ADDTBLS
* После успешного создания миграции, VS откроет файл миграции для ознакомления. 
В демке используется три модели (= три таблицы в БД), две из которых имеют внешний ключ на первую. В файле миграции для таблиц будет задано RefentialAction.Cascade - удалить зависимую таблицу при удалении связанного ключа (FK). Менять действие не нужно - зависимые таблицы содержат адрес и мессенджеры, дополняющие основную карточку контакта, удаляются вслед за основной карточкой.
* в CMD (PM>) набрать и выполнить команду update-database. 
* установить Contacts.web в качестве startup-проекта и запустить приложение. Демо контакт будет создан автоматически, если не отключён DbSeed();

3) Ручное наполнение
для проверки вручную добавьте через веб два контакта, задав разные права доступа к адресам и/или мессенджерам через API.
Веб можно использовать для изменения прав (аля админка) и проверки.

4) использование API
Переключить Startup Project на Contacts.API, запустить приложение. Откроется Swagger, его можно использовать для проверки, в том числе посмотреть модели данных DTO и возможные ответы (ProducesResponses).
API endpoint - /api/ContactAPI : https://localhost:7031/api/ContactAPI
Доступные методы:
GET /api/ContactAPI - все контакты (основные карточки)
POST /api/ContactAPI - добавить контакт
PUT /api/ContactAPI - обновить контакт
GET /api/ContactAPI/Id - получить данные контакта (основная карточка и адрес и мессенджеры, если разрешено получение по API)
DELETE /api/ContactAPI/Id - Удалить контакт.

Для метода POST /api/ContactAPI применяется проверка модели. В случае ошибок в ответ будут переданы подробности ошибки
В методе PUT выполняется проверка введённых данных: корректность Id, наличие контакта в БД. При наличии конакта в БД, сверяем каждое поле из DTO с таким же полем из БД конкретного контакта. Если различаются, то сохраняем в БД новое значение. Для некоторых полей в БД дополнительно проверяются граничные условия (например, в поле Имя или Фамилия нельзя задать пустую строку). 


Комментарии:
Contacts.Web:
1) В версии 1.0 не реализовал:
* identity
* iformfile и загрузку фото через веб и через API. Загрузку файла/фото через веб-форму реализовывал, через API загрузку файла ещё не тестировал;
2) Красоту не наводил, веб-компонента сделана для проверки и демонстрации, что концепцию MVC понимаю;
3) чекбоксы (bool переменные) из формы через Viewmodel не передаются в контроллер, но отлично распознаются как независимые переменные - на входе в контроллер принимаю Viewmodel и две bool-переменные, которые внутри контроллера сохраняю в БД. При этом из контроллера модель с bool значениями корректно заполняет форму и проставляет галочки в чекбоксах. Кажется это стандартное поведение;
4) В слое Application нет программной (бизнес-) логики. В рамках демо сложную логику не делал. Для сохранения данных используется слой Infrastructure, для моделей данных и объектов передачи данных (Model/ViewModel и DTO соответственно) - Contacts.Domain. 
Слои Contacts.API и Contacts.Web находятся на внешнем уровне и зависят от Infrastructure, который зависит от Contacts.Domain
5) repository pattern (UnitOfWork) не применял. В данном случае достаточно через Dependency Injection встроить ApplicationDbContext. 
6) в методах, выполняющих только чтение, запросы в БД не отслеживаются: AsNoTracking().

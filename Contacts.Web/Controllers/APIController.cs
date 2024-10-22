using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contacts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Contacts.Domain.Entities;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using Contacts.Domain.Entities.DTO;
using Contacts.Application.BL;

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
        private readonly ILogic _logic;
        public APIController(ILogic logic)
        {
            _logic = logic;
        }

        // Endpoints. Every endpoint contains declaration of possible responses
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Contact>> ContactList()
        {
            var ContactList = _logic.List();
            if (ContactList.Count() == 0)
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
            if (Id == 0)
            { return BadRequest(); }

            var contactDTO = _logic.Get(Id);
            if (contactDTO.ContactId == 0)
            {
                return NotFound();
            }
            return Ok(contactDTO);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
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
                return StatusCode(StatusCodes.Status500InternalServerError, "ID isn't zero. Can't create contact");
            }

            var result = _logic.Add(contactDTO);
            if (result > 0)
            {
                return Ok();
//                return CreatedAtRoute("Contact", new { Id = result }, contactDTO);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("Delete", Name = "DeleteContact")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteContact(UInt64 Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var result = _logic.Delete(Id);
            if (result == 1)
            {
                return NotFound();
            }
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

            var result = _logic.Update(contactDTO);
            if (result == 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
    }
}

using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        //Database connection
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        //Get all the contracts asynchronously
        public async Task<IEnumerable<Contact>> GetContacts()
        {
            return await dbContext.Contacts.ToListAsync();
            
        }

        [HttpPost]
        //Create a contact asynchronously
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest )
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone
            };

            //Put it in the database and save it using async method
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact);
        }
        
        [HttpPut]
        [Route("{id:guid}")]
        //Update the contact asynchronously
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null) //if contact exists
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Address = updateContactRequest.Address;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;

                await dbContext.SaveChangesAsync();

                return Ok(contact);
                
            }

            return NotFound();
        }

        [HttpGet]
        [Route("{id:guid}")]
        //Get the specific contact asynchronously
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null) //if contact doesnt exist
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //Delete the specific contact asynchronously
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
        
            if(contact !=null) //if contact exists
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }

            return NotFound();
        }
    }
}

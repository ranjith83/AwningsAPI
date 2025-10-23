using AwningsAPI.Dto.Customers;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomersWithContacts()
        {
            var customers = await _customerService.GetAllCustomersWithContactsAsync();

            var customerDtos = customers.Select(c => new CustomerMainViewDto
            {
                CompanyId = c.CompanyId,
                CompanyName = c.Name ?? string.Empty,
                ContactName = c.CustomerContacts?.FirstOrDefault()?.FirstName + ' ' + c.CustomerContacts?.FirstOrDefault()?.LastName ?? string.Empty,
                ContactEmail = c.CustomerContacts?.FirstOrDefault()?.Email ?? string.Empty,
                MobilePhone = c.Mobile ?? string.Empty,
                SiteAddress = string.Join(", ", new[] { c.Address1, c.Address2, c.Address3 }
                              .Where(a => !string.IsNullOrWhiteSpace(a)))
            }).ToList();


            return Ok(customerDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost("add-company-with-contact")]
        public async Task<IActionResult> AddCompanyWithContact([FromBody] CompanyWithContactDto dto)
        {
            var customer = await _customerService.SaveCompanyWithContact(dto);

            if (customer == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(AddCompanyWithContact), new { Id = customer.CompanyId }, customer);
        }

        [HttpPost("add-contact-to-company")]
        public async Task<IActionResult> AddContactToCompany([FromBody] ContactDto dto)
        {
            var customer = await _customerService.SaveContactToCompany(dto);

            if (customer == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(AddContactToCompany), new { Id = customer.CompanyId }, customer);
        }

        [HttpPut("update-company/{companyId}")]
        public async Task<IActionResult> UpdateCompany(int companyId, [FromBody] CompanyDto dto)
        {
            var company = await _customerService.UpdateCompany(companyId, dto);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPut("update-contact/{contactId}")]
        public async Task<IActionResult> UpdateContact(int contactId, [FromBody] ContactDto dto)
        {
            var customer = await _customerService.UpdateContactInCompany(contactId, dto);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
    }
}

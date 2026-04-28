using AwningsAPI.Dto.Customers;
using AwningsAPI.Dto.Eircode;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomersWithContacts()
        {
            var customers = await _customerService.GetAllCustomersWithContactsAsync();

            var customerDtos = customers.Select(c => new CustomerMainViewDto
            {
                CustomerId = c.CustomerId,
                CompanyName = c.Name ?? string.Empty,
                ContactName = c.CustomerContacts?.FirstOrDefault()?.FirstName + " " +
                             c.CustomerContacts?.FirstOrDefault()?.LastName ?? string.Empty,
                ContactEmail = c.CustomerContacts?.FirstOrDefault()?.Email ?? string.Empty,
                MobilePhone = c.Mobile ?? string.Empty,
                Email = c.Email ?? string.Empty,
                Phone = c.Phone ?? string.Empty,
                SiteAddress = string.Join(", ", new[] { c.Address1, c.Address2, c.Address3 }
                              .Where(a => !string.IsNullOrWhiteSpace(a))),
                AssignedSalesperson = c.AssignedSalespersonName ?? string.Empty
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
            var currentUser = User?.Identity?.Name ?? "System";
            var customer = await _customerService.SaveCompanyWithContact(dto, currentUser);
            if (customer == null) return NotFound();
            _logger.LogInformation("Customer {CustomerId} created by {User}", customer.CustomerId, currentUser);
            return CreatedAtAction(nameof(AddCompanyWithContact), new { Id = customer.CustomerId }, customer);
        }

        [HttpPost("add-contact-to-company")]
        public async Task<IActionResult> AddContactToCompany([FromBody] ContactDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var customer = await _customerService.SaveContactToCompany(dto, currentUser);
            if (customer == null) return NotFound();
            _logger.LogInformation("Contact added to customer {CustomerId} by {User}", dto.CompanyId, currentUser);
            return CreatedAtAction(nameof(AddContactToCompany), new { Id = customer.CustomerId }, customer);
        }

        [HttpPut("update-company/{companyId}")]
        public async Task<IActionResult> UpdateCompany(int companyId, [FromBody] CompanyDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var company = await _customerService.UpdateCompany(companyId, dto, currentUser);
            if (company == null) return NotFound();
            _logger.LogInformation("Customer {CustomerId} updated by {User}", companyId, currentUser);
            return Ok(company);
        }

        [HttpPut("update-contact/{contactId}")]
        public async Task<IActionResult> UpdateContact(int contactId, [FromBody] ContactDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var customer = await _customerService.UpdateContactInCompany(contactId, dto, currentUser);
            if (customer == null) return NotFound();
            _logger.LogInformation("Contact {ContactId} updated by {User}", contactId, currentUser);
            return Ok(customer);
        }

        [HttpPost("delete-customer")]
        public async Task<IActionResult> DeleteCustomerWithContact([FromBody] int customerId)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var customer = await _customerService.DeleteCompanyWithContact(customerId, currentUser);
            if (customer == null) return NotFound();
            _logger.LogInformation("Customer {CustomerId} deleted by {User}", customerId, currentUser);
            return Ok(customer);
        }

        [Authorize]
        [HttpGet("eircode-lookup/{eircode}")]
        public async Task<ActionResult<EircodeResultDto>> EircodeLookup(string eircode)
        {
            var result = await _customerService.LookupEircodeAsync(eircode);
            if (result == null)
                return NotFound("No address found for the given Eircode.");

            return Ok(result);
        }
    }
}
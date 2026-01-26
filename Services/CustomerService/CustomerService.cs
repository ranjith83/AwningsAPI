using AwningsAPI.Database;
using AwningsAPI.Dto.Customers;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Customers;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<IEnumerable<Customer>> GetAllCustomersWithContactsAsync()
        {
            return await _context.Customers.Include(c => c.CustomerContacts).ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            var company = await _context.Customers
                .Include(c => c.CustomerContacts)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (company is null)
                throw new KeyNotFoundException($"Company with id {customerId} was not found.");

            return company;
        }

        public async Task<Customer> SaveCompanyWithContact(CompanyWithContactDto dto, string currentUser)
        {
            var customer = new Model.Customers.Customer
            {
                Name = dto.Name,
                CompanyNumber = dto.CompanyNumber,
                Residential = dto.Residential,
                RegistrationNumber = dto.RegistrationNumber,
                VATNumber = dto.VATNumber,
                Address1 = dto.Address1,
                Address2 = dto.Address2,
                Address3 = dto.Address3,
                County = dto.County,
                CountryId = dto.CountryId,
                Phone = dto.Phone,
                Fax = dto.Fax,
                Mobile = dto.Mobile,
                Email = dto.Email,
                TaxNumber = dto.TaxNumber,
                Eircode = dto.Eircode,
                AssignedSalespersonId = dto.AssignedSalespersonId,
                AssignedSalespersonName = dto.AssignedSalespersonName,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser,
                CustomerContacts = dto.Contacts.Select(c => new CustomerContact
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Phone = c.Phone,
                    Email = c.Email,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = currentUser
                }).ToList()
            };

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<CustomerContact> SaveContactToCompany(ContactDto dto, string currentUser)
        {
            var customerContact = new CustomerContact
            {
                CustomerId = dto.CompanyId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Mobile = dto.Mobile,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            _context.CustomerContacts.Add(customerContact);

            await _context.SaveChangesAsync();

            return customerContact;
        }

        public async Task<Customer> UpdateCompany(int CompanyId, CompanyDto dto, string currentUser)
        {
            var company = await _context.Customers.FindAsync(CompanyId);

            if (company == null)
            {
                throw new KeyNotFoundException($"Company with ID {CompanyId} not found.");
            }

            company.Name = dto.Name;
            company.CompanyNumber = dto.CompanyNumber;
            company.Residential = dto.Residential;
            company.RegistrationNumber = dto.RegistrationNumber;
            company.VATNumber = dto.VATNumber;
            company.Address1 = dto.Address1;
            company.Address2 = dto.Address2;
            company.Address3 = dto.Address3;
            company.County = dto.County;
            company.CountryId = dto.CountryId;
            company.Phone = dto.Phone;
            company.Fax = dto.Fax;
            company.Mobile = dto.Mobile;
            company.Email = dto.Email;
            company.TaxNumber = dto.TaxNumber;
            company.Eircode = dto.Eircode;
            company.AssignedSalespersonId = dto.AssignedSalespersonId;
            company.AssignedSalespersonName = dto.AssignedSalespersonName;
            company.UpdatedDate = DateTime.UtcNow;
            company.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<CustomerContact> UpdateContactInCompany(int contactId, ContactDto dto, string currentUser)
        {
            var contact = await _context.CustomerContacts.FindAsync(contactId);

            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact with ID {contactId} not found.");
            }

            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.Email = dto.Email;
            contact.Phone = dto.Phone;
            contact.DateOfBirth = dto.DateOfBirth;
            contact.DateUpdated = DateTime.UtcNow;
            contact.UpdatedBy = currentUser;

            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> DeleteCompanyWithContact(int customerId, string currentUser)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer Id {customerId} not found.");
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
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

        public async Task<Customer> GetCustomerByIdAsync(int companyId)
        {
            var company = await _context.Customers
                .Include(c => c.CustomerContacts)
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);

            if (company is null)
                throw new KeyNotFoundException($"Company with id {companyId} was not found.");

            return company;
        }

        public async Task<Customer> SaveCompanyWithContact(CompanyWithContactDto dto)
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
                DateCreated = DateTime.UtcNow,
                CustomerContacts = dto.Contacts.Select(c => new CustomerContact
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Phone = c.Phone,
                    Email = c.Email
                }).ToList()
            };

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<CustomerContact> SaveContactToCompany(ContactDto dto)
        {
            var customerContact = new CustomerContact
            {
                CompanyId = dto.CompanyId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _context.CustomerContacts.Add(customerContact);

            await _context.SaveChangesAsync();

            return customerContact;
        }

        public async Task<Customer> UpdateCompany(int CompanyId, CompanyDto dto)
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
            company.UpdatedDate = DateTime.UtcNow;
            company.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();

            return company;
        }
        public async Task<CustomerContact> UpdateContactInCompany(int contactId, ContactDto dto)
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
            contact.UpdatedBy = dto.UpdatedBy;

            await _context.SaveChangesAsync();

            return contact;
        }
    }
}

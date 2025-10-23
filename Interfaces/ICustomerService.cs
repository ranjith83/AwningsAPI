using AwningsAPI.Dto.Customers;
using AwningsAPI.Model.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersWithContactsAsync();
        Task<Customer> GetCustomerByIdAsync(int id);

        Task<Customer> SaveCompanyWithContact(CompanyWithContactDto dto);

        Task<CustomerContact> SaveContactToCompany(ContactDto dto);
        Task<Customer> UpdateCompany(int id, CompanyDto dto);
        Task<CustomerContact> UpdateContactInCompany(int id, ContactDto dto);  
    }
}

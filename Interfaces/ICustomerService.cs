using AwningsAPI.Dto.Customers;
using AwningsAPI.Model.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersWithContactsAsync();
        Task<Customer> GetCustomerByIdAsync(int id);

        Task<Customer> SaveCompanyWithContact(CompanyWithContactDto dto, string currentUser);

        Task<CustomerContact> SaveContactToCompany(ContactDto dto, string currentUser);
        Task<Customer> UpdateCompany(int id, CompanyDto dto, string currentUser);
        Task<CustomerContact> UpdateContactInCompany(int id, ContactDto dto, string currentUser);

        Task<bool> DeleteCompanyWithContact(int customerId, string currentUser);
    }
}

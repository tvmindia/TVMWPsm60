using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ICustomerBusiness
    {
        bool CheckCompanyNameExist(string companyName);
        bool CheckCustomerEmailExist(string contactEmail);
        bool CheckMobileNumberExist(string mobile);
        List<Titles> GetAllTitles();
        List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch);
        Customer GetCustomer(Guid id);
        object InsertUpdateCustomer(Customer customer);
        object DeleteCustomer(Guid id);
    }
}

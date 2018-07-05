using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ICustomerBusiness
    {
        bool CheckCompanyNameExistForCustomer(Customer customer);
        bool CheckCustomerEmailExist(Customer customer);
        bool CheckMobileNumberExist(Customer customer);
        List<SelectListItem> GetTitleSelectList();
        List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch);
        Customer GetCustomer(Guid id);
        object InsertUpdateCustomer(Customer customer);
        object DeleteCustomer(Guid id);
        List<SelectListItem> GetCustomerSelectList();
        List<SelectListItem> GetCustomerSelectListOnDemand();
    }
}

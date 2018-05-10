using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ICustomerCategoryBusiness
    {
        object InsertUpdateCustomerCategory(CustomerCategory customerCategory);
        List<CustomerCategory> GetAllCustomerCategory(CustomerCategoryAdvanceSearch customerCategoryAdvanceSearch);
        CustomerCategory GetCustomerCategory(int code);
        bool CheckCustomerCategoryNameExist(CustomerCategory customerCategory);
        object DeleteCustomerCategory(int code);
        List<SelectListItem> GetCustomerCategoryForSelectList();
    }
}

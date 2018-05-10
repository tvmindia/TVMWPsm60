using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class CustomerCategoryBusiness : ICustomerCategoryBusiness
    {
        private ICustomerCategoryRepository _customerCategoryRepository;
        public CustomerCategoryBusiness(ICustomerCategoryRepository customerCategoryRepository)
        {
            _customerCategoryRepository = customerCategoryRepository;
        }
        public object InsertUpdateCustomerCategory(CustomerCategory customerCategory)
        {
            return _customerCategoryRepository.InsertUpdateCustomerCategory(customerCategory);
        }
        public List<CustomerCategory> GetAllCustomerCategory(CustomerCategoryAdvanceSearch customerCategoryAdvanceSearch)
        {
            return _customerCategoryRepository.GetAllCustomerCategory(customerCategoryAdvanceSearch);
        }
        public CustomerCategory GetCustomerCategory(int code)
        {
            return _customerCategoryRepository.GetCustomerCategory(code);
        }
        public bool CheckCustomerCategoryNameExist(CustomerCategory customerCategory)
        {
            return _customerCategoryRepository.CheckCustomerCategoryNameExist(customerCategory);
        }
        public object DeleteCustomerCategory(int code)
        {
            return _customerCategoryRepository.DeleteCustomerCategory(code);
        }
        public List<SelectListItem> GetCustomerCategoryForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<CustomerCategory> customerCategoryList = _customerCategoryRepository.GetCustomerCategoryForSelectList();
            return selectListItem = customerCategoryList!=null?(from customerCategory in customerCategoryList
                                     select new SelectListItem
                                     {
                                         Text = customerCategory.Name,
                                         Value = customerCategory.Code.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
    }
}

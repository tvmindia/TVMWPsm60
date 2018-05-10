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
    public class CustomerBusiness:ICustomerBusiness
    {
        private ICustomerRepository _customerRepository;

        public bool CheckCompanyNameExistForCustomer(Customer customer)
        {
            return _customerRepository.CheckCompanyNameExistForCustomer(customer);
        }
        public bool CheckCustomerEmailExist(Customer customer)
        {
            return _customerRepository.CheckCustomerEmailExist(customer);
        }
        public bool CheckMobileNumberExist(Customer customer)
        {
            return _customerRepository.CheckMobileNumberExist(customer);
        }
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public List<SelectListItem> GetTitleSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Titles> titlesList = _customerRepository.GetAllTitles();
            return selectListItem = (from titles in titlesList
                                     select new SelectListItem
                                     {
                                         Text = titles.Title,
                                         Value = titles.Title,
                                         Selected = false
                                     }).ToList();
        }
        public Customer GetCustomer(Guid id)
        {
            return _customerRepository.GetCustomer(id);
        }
        public object InsertUpdateCustomer(Customer customer)
        {
            return _customerRepository.InsertUpdateCustomer(customer);
        }
        public List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch)
        {
            return _customerRepository.GetAllCustomer(customerAdvanceSearch);
        }
        public object DeleteCustomer(Guid id)
        {
            return _customerRepository.DeleteCustomer(id);
        }
        public List<SelectListItem> GetCustomerSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Customer> customerList =_customerRepository.GetCustomerSelectList();
            return selectListItem = customerList != null ? (from customer in customerList
                                                            select new SelectListItem
                                                            {
                                                                Text = customer.CompanyName,
                                                                Value = customer.ID.ToString(),
                                                                Selected = false
                                                            }).ToList():new List<SelectListItem>();
                
        }
    }
}

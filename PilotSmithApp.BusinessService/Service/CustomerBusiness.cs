using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class CustomerBusiness:ICustomerBusiness
    {
        private ICustomerRepository _customerRepository;

        public bool CheckCompanyNameExist(string companyName)
        {
            return _customerRepository.CheckCompanyNameExist(companyName);
        }
        public bool CheckCustomerEmailExist(string contactEmail)
        {
            return _customerRepository.CheckCustomerEmailExist(contactEmail);
        }
        public bool CheckMobileNumberExist(string mobile)
        {
            return _customerRepository.CheckMobileNumberExist(mobile);
        }
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public List<Titles> GetAllTitles()
        {
            return _customerRepository.GetAllTitles();
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
    }
}

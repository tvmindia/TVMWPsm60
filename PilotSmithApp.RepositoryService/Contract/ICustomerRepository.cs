﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface ICustomerRepository
    {
        List<Titles> GetAllTitles();
        object InsertUpdateCustomer(Customer customer);
        Customer GetCustomer(Guid id);
        List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch);
    }
}

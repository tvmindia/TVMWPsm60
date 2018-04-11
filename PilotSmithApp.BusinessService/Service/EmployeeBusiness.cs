﻿using PilotSmithApp.BusinessService.Contract;
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
    public class EmployeeBusiness:IEmployeeBusiness
    {
        IEmployeeRepository _employeeRepository;
        public EmployeeBusiness(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public List<SelectListItem> GetEmployeeSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Employee> employeeList = _employeeRepository.GetEmployeeSelectList();
            if (employeeList != null)
                foreach (Employee employee in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = employee.Code+" | "+employee.Name,
                        Value = employee.ID.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}

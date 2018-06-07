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
    public class EmployeeBusiness:IEmployeeBusiness
    {
        IEmployeeRepository _employeeRepository;
        public EmployeeBusiness(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public object InsertUpdateEmployee(Employee employee)
        {
            return _employeeRepository.InsertUpdateEmployee(employee);
        }
        public Employee GetEmployee(Guid id)
        {
            return _employeeRepository.GetEmployee(id);
        }
        public List<Employee> GetAllEmployee(EmployeeAdvanceSearch employeeAdvanceSearch)
        {
            return _employeeRepository.GetAllEmployee(employeeAdvanceSearch);
        }
        public object DeleteEmployee(Guid id)
        {
            return _employeeRepository.DeleteEmployee(id);
        }
        public List<SelectListItem> GetEmployeeSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Employee> employeeList = _employeeRepository.GetEmployeeSelectList();
            return selectListItem = employeeList!=null?(from employee in employeeList
                                     select new SelectListItem
                                     {
                                         Text = employee.Name,
                                         Value = employee.ID.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
        public bool CheckEmployeeCodeExist(Employee employee)
        {
            return _employeeRepository.CheckEmployeeCodeExist(employee);
        }

    }
}

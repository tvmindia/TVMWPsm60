using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IEmployeeBusiness
    {
        object InsertUpdateEmployee(Employee employee);
        List<Employee> GetAllEmployee(EmployeeAdvanceSearch employeeAdvanceSearch);
        Employee GetEmployee(Guid id);
        object DeleteEmployee(Guid id);
        List<SelectListItem> GetEmployeeSelectList();
        bool CheckEmployeeCodeExist(Employee employee);
    }
}

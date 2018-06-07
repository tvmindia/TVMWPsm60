using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IEmployeeRepository
    {
        object InsertUpdateEmployee(Employee employee);
        Employee GetEmployee(Guid id);
        List<Employee> GetAllEmployee(EmployeeAdvanceSearch employeeAdvanceSearch);
        object DeleteEmployee(Guid id);
        List<Employee> GetEmployeeSelectList();
        bool CheckEmployeeCodeExist(Employee employee);
    }
}

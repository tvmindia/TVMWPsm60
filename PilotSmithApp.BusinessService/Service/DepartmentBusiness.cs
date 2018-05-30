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
    public class DepartmentBusiness: IDepartmentBusiness
    {
        IDepartmentRepository _departmentRepository;
        public DepartmentBusiness(IDepartmentRepository departmentRepository)
    {
            _departmentRepository = departmentRepository;
    }
    
        public List<SelectListItem> GetDepartmentSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Department> departmentList = _departmentRepository.GetDepartmentSelectList();
            return selectListItem = departmentList != null ? (from department in departmentList
                                                                         select new SelectListItem
                                                                         {
                                                                             Text = department.Description,
                                                                             Value = department.Code.ToString(),
                                                                             Selected = false
                                                                         }).ToList() : new List<SelectListItem>();
        }
    }
}

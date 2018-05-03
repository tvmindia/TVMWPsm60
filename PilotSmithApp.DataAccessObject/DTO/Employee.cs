using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Employee
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string ImageURL { get; set; }
        public int? DepartmentCode { get; set; }
        public int? PositionCode { get; set; }
        public bool IsActive { get; set; }
        public string GeneralNotes { get; set; }

        //Additiona Fields
        public PSASysCommon PSASysCommon { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }

    public class EmployeeAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class EmployeeViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Employee Code is missing")]
        [Remote(action: "CheckEmployeeCodeExist", controller: "Employee", AdditionalFields = "IsUpdate,ID")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Employee Name is missing")]
        public string Name { get; set; }
        [Display(Name = "Mobile")]
        [StringLength(50, MinimumLength = 5)]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Entered phone format is not valid.")]
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string ImageURL { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentCode { get; set; }
        [Display(Name = "Position")]
        public int? PositionCode { get; set; }
        public bool IsActive { get; set; }
        public string GeneralNotes { get; set; }
        public bool IsUpdate { get; set; }        
        public List<SelectListItem> EmployeeSelectList { get; set; }

        //Additional Fields
        public DepartmentViewModel Department { get; set; }
        public PositionViewModel Position { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }

    public class EmployeeAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentCode { get; set; }
        public DepartmentViewModel Department { get; set; }
        [Display(Name = "Position")]
        public int? PositionCode { get; set; }
        public PositionViewModel Position { get; set; }
    }
}
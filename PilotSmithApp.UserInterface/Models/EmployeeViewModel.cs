using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class EmployeeViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Employee Code is missing")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Employee Name is missing")]
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string ImageURL { get; set; }
        public int? DepartmentCode { get; set; }
        public int? PositionCode { get; set; }
        public bool IsActive { get; set; }
        public string GeneralNotes { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}
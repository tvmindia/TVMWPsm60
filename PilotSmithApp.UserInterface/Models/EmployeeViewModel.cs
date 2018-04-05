using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class EmployeeViewModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string ImageURL { get; set; }
        public int DepartmentCode { get; set; }
        public int PositionCode { get; set; }
        public bool IsActive { get; set; }
        public string GeneralNotes { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}
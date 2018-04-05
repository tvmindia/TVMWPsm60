using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class ReferencePersonViewModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int ReferenceTypeCode { get; set; }
        public int AreaCode { get; set; }
        public string Organization { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNos { get; set; }
        public string FaxNos { get; set; }
        public string GeneralNotes { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
}
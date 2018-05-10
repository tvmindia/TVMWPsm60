using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class ReferencePersonViewModel
    {
        public int Code { get; set; }
        [Remote(action: "CheckReferencePersonNameExist", controller: "ReferencePerson", AdditionalFields = "IsUpdate,Code")]
        [Required(ErrorMessage = "Name is missing")]
        public string Name { get; set; }
        [Display(Name = "Reference Type")]
        public int? ReferenceTypeCode { get; set; }
        [Display(Name = "Area")]
        public int? AreaCode { get; set; }
        public string Organization { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }
        [Display(Name = "Phone")]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNos { get; set; }
        [Display(Name = "Fax")]
        public string FaxNos { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> ReferencePersonSelectList { get; set; }
        //Additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public ReferenceTypeViewModel ReferenceType { get; set; }
        public AreaViewModel Area { get; set; }
    }
    public class ReferencePersonAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Reference Type")]
        public int? ReferenceTypeCode { get; set; }
        public ReferenceTypeViewModel ReferenceType { get; set; }
        [Display(Name = "Area")]
        public int? AreaCode { get; set; }
        public AreaViewModel Area { get; set; }

    }
}
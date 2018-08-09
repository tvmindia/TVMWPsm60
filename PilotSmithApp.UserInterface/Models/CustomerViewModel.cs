using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Customer is missing")]
        [Display(Name = "Customer")]
        public Guid ID { get; set; }
        [Display(Name = "Company Name")]
        [MaxLength(150)]
        [Required(ErrorMessage = "Company Name is missing")]
        [Remote(action: "CheckCompanyNameExistForCustomer", controller: "Customer", AdditionalFields = "IsUpdate,ID")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact Person")]
        [MaxLength(100)]
        public string ContactPerson { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$", ErrorMessage = "Enter a valid e-mail adress")]
        [MaxLength(150)]
        //[Remote(action: "CheckCustomerEmailExist", controller: "Customer", AdditionalFields = nameof(IsUpdate))]
        public string ContactEmail { get; set; }
        [Display(Name = "Title")] 
        [MaxLength(10)]
        public string ContactTitle { get; set; }
        [Display(Name = "Website")]
        [MaxLength(500)]
        public string Website { get; set; }
        [Display(Name = "Phone")]
        [StringLength(50, MinimumLength = 5)]
        public string LandLine { get; set; }
        [Display(Name = "Mobile")]
        [StringLength(50, MinimumLength = 5)]
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Entered phone format is not valid.")]
        //[Remote(action: "CheckMobileNumberExist", controller: "Customer", AdditionalFields = nameof(IsUpdate))]
        public string Mobile { get; set; }
        [Display(Name = "Fax")]
        [StringLength(50, MinimumLength = 5)]
        public string Fax { get; set; }
        [Display(Name = "Other Number(s) if any")]
        [MaxLength(250)]
        public string OtherPhoneNos { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Default Payment Term")]
        [MaxLength(10)]
        public string PaymentTermCode { get; set; }
        [Display(Name = "Tax Registration Number")]
        [MaxLength(50)]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage ="Entered GSTIN is not valid")]
        public string TaxRegNo { get; set; }
        [Display(Name = "Pan Number")]
        [MaxLength(50)]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage ="Entered PAN number is not valid")]
        public string PANNO { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Available Advance Amount:")]
        public decimal AdvanceAmount { get; set; }
        public decimal OutStanding { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public List<SelectListItem> CustomerSelectList { get; set; }
        public TitlesViewModel Titles { get; set; }
        public PaymentTermViewModel PaymentTerm { get; set; }
        [Display(Name ="Area")]
        public int? AreaCode { get; set; }
        [Display(Name = "District")]
        public int? DistrictCode { get; set; }
        [Display(Name = "State")]
        public int? StateCode { get; set; }
        [Display(Name = "Country")]
        public int? CountryCode { get; set; }
        [Display(Name = "Aadhar Number")]        
        [MaxLength(20)]
        public string AadharNo { get; set; }
        public string TallyName { get; set; }

        //Additional properties
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        [Display(Name ="Category")]
        public int[] CustomerCategoryList { get; set; }
        public CustomerCategoryViewModel CustomerCategory { get; set; }
        //public int CustomerCategoryCode { get; set; }      
        public string CustomerSelect { get; set; }
        public List<SelectListItem> CustomerList { get; set; }


    }
    public class CustomerAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        [Display(Name ="From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Is internal company")]
        public string IsInternalComp { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
    public class TitlesViewModel
    {
        public string Title { get; set; }
        public List<SelectListItem> TitlesSelectList { get; set; }
    }
}
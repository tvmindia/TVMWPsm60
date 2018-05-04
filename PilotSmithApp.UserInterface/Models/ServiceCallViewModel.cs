using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PilotSmithApp.UserInterface.Models
{
    public class ServiceCallViewModel
    {
        public Guid ID { get; set; }
        public string ServiceCallNo { get; set; }
        public DateTime ServiceCallDate { get; set; }
        public TimeSpan? ServiceCallTime { get; set; }
        public Guid? CustomerID { get; set; }
        public Guid? AttendedBy { get; set; }
        public string CalledPersonName { get; set; }
        public int? DocumentStatusCode { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? ServicedBy { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ServiceComments { get; set; }

        //Additional Fields
        public List<ServiceCallDetailViewModel> ServiceCallDetailList { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public string ServiceCallDateFormatted { get; set; }
        public string ServiceDateFormatted { get; set; }
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CustomerViewModel Customer { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
    }

    public class ServiceCallAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }

    public class ServiceCallDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid ServiceCallID{get;set;}
        public Guid? ProductID{get;set;}
        public Guid? ProductModelID{get;set;}
        public string ProductSpec{get;set;}
        public bool? GuaranteeYN{get;set;}
        public DateTime? InstalledDate{get;set;}
        public int? ServiceStatusCode{get;set;}
        public bool IsUpdate { get; set; }

        //Additional Field
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public string InstalledDateFormatted { get; set; }
    }
}

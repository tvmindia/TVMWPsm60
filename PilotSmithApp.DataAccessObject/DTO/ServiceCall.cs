using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class ServiceCall
    {
        public Guid ID { get; set; }
        public string ServiceCallNo { get; set; }
        public DateTime ServiceCallDate { get; set; }
        public DateTime? ServiceCallTime { get; set; }
        public Guid? CustomerID { get; set; }
        public Guid? AttendedBy { get; set; }
        public string CalledPersonName { get; set; }
        public int? DocumentStatusCode { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? ServicedBy { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ServiceComments { get; set; }
        public int? BranchCode { get; set; }

        //Additional Fields
        public List<ServiceCallDetail> ServiceCallDetailList { get; set; }
        public List<ServiceCallCharge> ServiceCallChargeList { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public string ServiceCallDateFormatted { get; set; }
        public string ServiceCallTimeFormatted { get; set; }
        public string ServiceDateFormatted { get; set; }
        public string DetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string CallChargeXML { get; set; }
        public string ServicedByName { get; set; }
        public Branch Branch { get; set; }
        public Area Area { get; set; }
    }

    public class ServiceCallAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public Guid AdvCustomerID { get; set; }
        public Guid AdvServicedBy { get; set; }
        public Guid AdvAttendedBy { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public DataTablePaging DataTablePaging { get; set; }

        public Customer AdvCustomer { get; set; }
        public Area AdvArea { get; set; }
        public Employee AdvEmployee { get; set; }
        public DocumentStatus AdvDocumentStatus { get; set; }
        public Branch AdvBranch { get; set; }
    }

    public class ServiceCallDetail
    {
        public Guid ID { get; set; }
        public Guid ServiceCallID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public bool? GuaranteeYN { get; set; }
        public DateTime? InstalledDate { get; set; }
        public int? ServiceStatusCode { get; set; }
        public bool IsUpdate { get; set; }

        //Additional Fields
        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public string InstalledDateFormatted { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
    }

    public class ServiceCallCharge
    {
        public Guid ID { get; set; }
        public Guid? ServiceCallID { get; set; }
        public int? OtherChargeCode { get; set; }
        public decimal? ChargeAmount { get; set; }
        public int? TaxTypeCode { get; set; }
        public decimal? CGSTPerc { get; set; }
        public decimal? SGSTPerc { get; set; }
        public decimal? IGSTPerc { get; set; }
        public decimal? AddlTaxPerc { get; set; }
        public decimal? AddlTaxAmt { get; set; }

        //Additional Field
        public bool IsUpdate { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public TaxType TaxType { get; set; }
        public OtherCharge OtherCharge { get; set; }
    }
}

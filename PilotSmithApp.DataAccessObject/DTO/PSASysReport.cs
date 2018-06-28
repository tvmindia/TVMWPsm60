using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
   public class PSASysReport
    {
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReportGroup { get; set; }
        public int GroupOrder { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int ReportOrder { get; set; }
        public List<PSASysReport> PSASysReportList { get; set; }
        public string SearchTerm { get; set; }
    }

    public class PendingSaleOrderProductionReport
    {
        public Guid? SaleOrderID { get; set; }
        public decimal? PendingQty { get; set; }
        public decimal? SaleOrderQty { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? ProducedQty { get; set; }
        public string SaleOrderNo { get; set; }        
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public Customer Customer { get; set; }
        public Guid AdvCustomerID { get; set; }
        public Product Product { get; set; }
        public Guid? AdvProductID { get; set; }
        public string AdvSaleOrderNo { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<PendingSaleOrderProductionReport> PendingSaleOrderProductionReportList { get; set; }
    }

    public class EnquiryReport
    {
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryDateFormatted { get; set; }
        public string RequirementSpec { get; set; }
        public string CustomerID { get; set; }
        public EnquiryGrade EnquiryGrade { get; set; }
        public int? AdvEnquiryGradeCode { get; set; }
        public int? DocumentStatusCode { get; set; }
        public int? ReferredByCode { get; set; }  
        public Guid? AdvAttendedByID { get; set; }    
        public Employee Employee { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        public decimal? Amount { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid AdvCustomerID { get; set; }
        public Customer Customer { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
        public int? AdvCountryCode { get; set; }
        public int? AdvAreaCode { get; set; }
        public Area Area { get; set; }
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public int? AdvBranchCode { get; set; }
        public Branch Branch { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUser PSAUser { get; set; }
        public decimal? AdvAmountFrom { get; set; }
        public decimal? AdvAmountTo { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int? AdvReferenceTypeCode { get; set; }
        public CustomerCategory CustomerCategory { get; set; }
        public int? AdvCustomerCategoryCode { get; set; }
        public string SearchTerm { get; set; }
        public List<EnquiryReport> EnquiryReportList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
}

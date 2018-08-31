using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Enquiry
    {
        public Guid ID { get; set; }
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string RequirementSpec { get; set; }
        public Guid? CustomerID { get; set; }
        public int? EnquiryGradeCode { get; set; }
        public int? DocumentStatusCode { get; set; }
        public int? ReferredByCode { get; set; }
        public Guid? ResponsiblePersonID { get; set; }
        public Guid? AttendedByID { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        public string DetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        //Additional properties
        public string EnquiryDateFormatted { get; set;}
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public Customer Customer { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public EnquiryGrade EnquiryGrade { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public PSAUser PSAUser { get; set; }
        public Branch Branch { get; set; }
        public Area Area { get; set;}
        public PSASysCommon PSASysCommon { get; set; }
        public List<EnquiryDetail> EnquiryDetailList { get; set; }
    }
    public class EnquiryAdvanceSearch
    {
        public string EnquiryDate { get; set; }
        public string SearchTerm { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid AdvCustomerID { get; set; }
        public Customer Customer { get; set; }
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
    }
    public class EnquiryDetail
    {
        public Guid ID { get; set; }
        public Guid EnquiryID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public Guid SpecTag { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Unit Unit { get; set; }
    }
    public class EnquirySummary
    {
        public int TotalEnquiryCount { get; set; }
        public int ConvertedEnquiryCount { get; set; }
    }
    public class EnquiryValueFolloupSummary {
        public string Enquiry { get; set; }
        public decimal EnquiryValue { get; set; }
        public int FollowupCount { get; set; }
    }
    public class EnquiryCountSummary
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public int EnquiryCount { get; set; }
    }
}

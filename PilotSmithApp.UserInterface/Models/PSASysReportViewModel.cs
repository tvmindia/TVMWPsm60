using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Models
{
    public class PSASysReportViewModel
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
        public List<PSASysReportViewModel> PSASysReportList { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
    }
    public class PendingSaleOrderProductionReportViewModel
    {
        public Guid? SaleOrderID { get; set; }
        public decimal? PendingQty { get; set; }
        public decimal? SaleOrderQty { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? ProducedQty { get; set; }
        public string SaleOrderNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string AdvFromDate { get; set; }
        [Display(Name = "To Date")]
        public string AdvToDate { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "Customer")]
        public Guid AdvCustomerID { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Product")]
        public Guid? AdvProductID { get; set; }
        [Display(Name = "Sale Order No")]
        public string AdvSaleOrderNo { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<PendingSaleOrderProductionReportViewModel> PendingSaleOrderProductionReportVMList { get; set; }
    }

    public class EnquiryReportViewModel   
    {
        public string EnquiryNo { get; set; }
        public string EnqNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryDateFormatted { get; set; }
        public string RequirementSpec { get; set; }
        public Guid? CustomerID { get; set; }
        [Display(Name = "Grade")]
        public int? AdvEnquiryGradeCode { get; set; }
        public EnquiryGradeViewModel EnquiryGrade { get; set; }
        public int? DocumentStatusCode { get; set; }
        public int? ReferredByCode { get; set; }
        [Display(Name = "Attended By")]
        public Guid? AdvAttendedByID { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        public decimal? Amount { get; set; }
        [Display(Name = "Enquiry From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Enquiry To")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        [Display(Name = "Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
        public ReferenceTypeViewModel ReferenceType { get; set; }
        [Display(Name = "Reference Type")]
        public int? AdvReferenceTypeCode { get; set; }
        public CustomerCategoryViewModel CustomerCategory { get; set; }
        [Display(Name = "Customer Catogery")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public List<EnquiryReportViewModel> EnquiryReportList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }

    public class EnquiryFollowupReportViewModel
    {
        public DateTime FollowupDate { get; set; }
        public DateTime FollowupTime { get; set; }
        public int PriorityCode { get; set; }
        public string Priority { get; set; }
        public string EnquiryNo { get; set; }
        public string EnqNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryDateFormatted { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string FollowupDateFormatted { get; set; }
        public string FollowupTimeFormatted { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "Followup From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Followup To")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "Status")]
        public string AdvStatus { get; set; }
        [Display(Name = "Priority")]
        public int? AdvFollowupPriority { get; set; }
        public string FollowupRemarks { get; set; }
        public List<EnquiryFollowupReportViewModel> EnquiryFollowupReportList { get; set; }
    }

    public class EstimateReportViewModel
    {
        public string EstimateNo { get; set; }
        public string EstNo { get; set; }
        public DateTime EstimateDate { get; set; }
        public string EstimateDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public decimal? Amount { get; set; }
        [Display(Name = "Estimate From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Estimate To")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        [Display(Name ="Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
        public CustomerCategoryViewModel CustomerCategory { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "Prepared By")]
        public Guid AdvPreparedBy { get; set; }
        public string Notes { get; set; }
        public string PreparedBy { get; set; }
        public List<EstimateReportViewModel> EstimateReportList { get; set; }
    }

    public class QuotationReportViewModel
    {
        public string QuoteNo { get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuoteDate { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "Quotation From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Quotation To")]
        public string AdvToDate { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "Prepared By")]
        public Guid AdvPreparedBy { get; set; }
        public string Notes { get; set; }
        public List<QuotationReportViewModel> QuotationReportList { get; set; }
        public string PreparedBy { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        public decimal? Amount { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string QuoteDateFormatted { get; set; }
        [Display(Name = "Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
    }

    public class PendingSaleOrderReportViewModel
    {
        public string SaleOrderNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? TaxableAmount { get; set; }
        public decimal? Amount { get; set; }
        public int? UnitCode { get; set; }            
        public UnitViewModel Unit { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Sale Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Sale Order To")]
        public string AdvToDate { get; set; }
        [Display(Name = "Exp. Delivery Date From")]
        public string AdvDelFromDate { get; set; }
        [Display(Name = "Exp. Delivery Date To")]
        public string AdvDelToDate { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        [Display(Name = "Prepared By")]
        public Guid AdvPreparedBy { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        [Display(Name = "Sale Order Amount (Incl.Tax) >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Sale Order Amount (Incl.Tax) <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Product")]
        public Guid AdvProduct { get; set; }
        public ProductViewModel Product { get; set; }        
        [Display(Name = "Product Model")]
        public Guid AdvProductModel { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }       
        public int? AdvReportType { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<PendingSaleOrderReportViewModel> PendingSaleOrderReportList { get; set; }
        public string SaleOrdNo { get; set; }
        public decimal? PendingQty { get; set; }
        public string DateFilter { get; set; }
        public int? ReportType { get; set; }

    }
    public class SaleOrderReportViewModel
    {
        public string SaleOrderNo { get; set; }
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public int? UnitCode { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Sale Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Sale Order To")]
        public string AdvToDate { get; set; }
        [Display(Name = "Exp. Delivery Date From")]
        public string AdvDelFromDate { get; set; }
        [Display(Name = "Exp. Delivery Date To")]
        public string AdvDelToDate { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        [Display(Name = "Prepared By")]
        public Guid AdvPreparedBy { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        [Display(Name = "Total Amount (Incl.Tax) >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Total Amount (Incl.Tax) <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }        
        public ProductViewModel Product { get; set; }
        [Display(Name = "Product")]
        public string AdvProduct { get; set; }
        [Display(Name = "Product Model")]
        public string AdvProductModel { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "Report Type")]
        public int? AdvReportType { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<SaleOrderReportViewModel> SaleOrderReportList { get; set; }
        public string SaleOrdNo { get; set; }        
        public string DateFilter { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }


    public class ProductionOrderReportViewModel
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrdNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public Guid? SaleOrderID { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public CustomerViewModel Customer { get; set; }
        
        [Display(Name = "Production Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Production Order To")]
        public string AdvToDate { get; set; }
        [Display(Name = "Exp. Delivery Date From")]
        public string AdvDelFromDate { get; set; }
        [Display(Name = "Exp. Delivery Date To")]
        public string AdvDelToDate { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Product")]
        public Guid AdvProduct { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Product Model")]
        public Guid AdvProductModel { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }
        public PlantViewModel Plant { get; set; }
        [Display(Name = "Plant")]
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }      
        public PSASysCommonViewModel PSASysCommon { get; set; }
        [Display(Name = "Prepared By")]
        public Guid AdvPreparedBy { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public decimal? Qty { get; set; }
    }

    public class ProductionOrderDetailForecastDateExceededReportViewModel
    {
            public string ProdOrderNo { get; set; }
            public string ProductionOrderNo { get; set; }
            public string SaleOrderNo { get; set; }
            public string SaleOrdNo { get; set; }
            public DateTime ProdOrderDate { get; set; }
            public string ProdOrderDateFormatted { get; set; }
            public Guid? SaleOrderID { get; set; }
            public Guid? CustomerID { get; set; }
            public DateTime? ExpectedDelvDate { get; set; }
            public string ExpectedDelvDateFormatted { get; set; }
            [Display(Name = "Referred By")]
            public int? AdvReferencePersonCode { get; set; }
            public ReferencePersonViewModel ReferencePerson { get; set; }
            public CustomerViewModel Customer { get; set; }

            [Display(Name = "Production Order From")]
            public string AdvFromDate { get; set; }
            [Display(Name = "Production Order To")]
            public string AdvToDate { get; set; }
            [Display(Name = "Exp. Delivery Date From")]
            public string AdvDelFromDate { get; set; }
            [Display(Name = "Exp. Delivery Date To")]
            public string AdvDelToDate { get; set; }
            [Display(Name = "Customer")]
            public string AdvCustomer { get; set; }
            public AreaViewModel Area { get; set; }
            [Display(Name = "Amount >=")]
            public decimal? AdvAmountFrom { get; set; }
            [Display(Name = "Amount <=")]
            public decimal? AdvAmountTo { get; set; }
            [Display(Name = "District")]
            public int? AdvDistrictCode { get; set; }
            [Display(Name = "State")]
            public int? AdvStateCode { get; set; }
            [Display(Name = "Country")]
            public int? AdvCountryCode { get; set; }
            [Display(Name = "Area")]
            public int? AdvAreaCode { get; set; }
            [Display(Name = "Customer Category")]
            public int? AdvCustomerCategoryCode { get; set; }
            [Display(Name = "Branch")]
            public int? AdvBranchCode { get; set; }
            public BranchViewModel Branch { get; set; }
            [Display(Name = "Document Status")]
            public int? AdvDocumentStatusCode { get; set; }
            public DocumentStatusViewModel DocumentStatus { get; set; }
            [Display(Name = "Document Owner")]
            public Guid AdvDocumentOwnerID { get; set; }
            public PSAUserViewModel PSAUser { get; set; }
            public int TotalCount { get; set; }
            public int FilteredCount { get; set; }
            [Display(Name = "Product")]
            public Guid AdvProduct { get; set; }
            public ProductViewModel Product { get; set; }
            [Display(Name = "Product Model")]
            public Guid AdvProductModel { get; set; }
            public ProductModelViewModel ProductModel { get; set; }
            public ApprovalStatusViewModel ApprovalStatus { get; set; }
            [Display(Name = "Approval Status")]
            public int? AdvApprovalStatusCode { get; set; }
            [Display(Name = "Search")]
            public string SearchTerm { get; set; }
            public string ProductSpec { get; set; }
            public string ProductName { get; set; }
            public string ProdModel { get; set; }
            public decimal? Amount { get; set; }
            public string Remarks { get; set; }
            public int? AdvReportType { get; set; }
            public int? AdvProgress { get; set; }
            public PlantViewModel Plant { get; set; }
            [Display(Name = "Plant")]
            public int? AdvPlantCode { get; set; }
            public string PreparedBy { get; set; }
            public string DateFilter { get; set; }
            public PSASysCommonViewModel PSASysCommon { get; set; }
            [Display(Name = "Prepared By")]
            public Guid AdvPreparedBy { get; set; }
            public EmployeeViewModel Employee { get; set; }
            public DataTablePagingViewModel DataTablePaging { get; set; }
            public decimal? Qty { get; set; }
            public decimal? ProducedQty { get; set; }
            public int? Progress { get; set; }
            public int? ExptProgress { get; set; }
            public int? ReportValue { get; set; }
            public string ExptCompletionDate { get; set; }

    }


    public class PendingProductionOrderReportViewModel
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrdNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }

        public DateTime ForecastDate { get; set; }
        public string ForecastDateFormatted { get; set; }
        public Guid? SaleOrderID { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "Production Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Production Order To")]
        public string AdvToDate { get; set; }
        [Display(Name = "Exp. Delivery Date From")]
        public string AdvDelFromDate { get; set; }
        [Display(Name = "Exp. Delivery Date To")]
        public string AdvDelToDate { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Product")]
        public Guid AdvProduct { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        [Display(Name = "Product Model")]
        public Guid AdvProductModel { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        [Display(Name = "Progress")]
        public int? AdvProgress { get; set; }
        [Display(Name = "Plant")]
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }      
        public string DateFilter { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }      
        public Guid AdvPreparedBy { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public PlantViewModel Plant { get; set; }
        public decimal? Qty { get; set; }
        public int? ReportType { get; set; }
        public decimal? PendingQty { get; set; }
        public decimal? SaleOrderQty { get; set; }
        public int? Progress { get; set; }
        public List<PendingProductionOrderReportViewModel> PendingProductionOrderReportList { get; set; }

    }

    public class ProductionQCStandardReportViewModel
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public string ProdQCNo { get; set; }
        public string ProductionQCNo { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }

        [Display(Name = "Production Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Production Order To")]
        public string AdvToDate { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Product")]
        public Guid AdvProduct { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        [Display(Name = "Product Model")]
        public Guid AdvProductModel { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }
        [Display(Name = "Plant")]
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public PlantViewModel Plant { get; set; }
        public int? ReportType { get; set; }
        public decimal? ProdOrdQty { get; set; }
        public decimal? ProdQCQty { get; set; }
        public List<ProductionQCStandardReportViewModel> ProductionQCStandardReportList { get; set; }
    }

    public class PendingProductionQCReportViewModel
    {
        public string ProdOrderNo { get; set; }
        public string ProductionOrderNo { get; set; }
        public DateTime ProdOrderDate { get; set; }
        public string ProdOrderDateFormatted { get; set; }
        public string ProdQCNo { get; set; }
        public string ProductionQCNo { get; set; }
        public Guid? CustomerID { get; set; }
        public DateTime? ExpectedDelvDate { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        public CustomerViewModel Customer { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }

        [Display(Name = "Production Order From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Production Order To")]
        public string AdvToDate { get; set; }
        public AreaViewModel Area { get; set; }
        [Display(Name = "Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Product")]
        public Guid AdvProduct { get; set; }
        public ProductViewModel Product { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        [Display(Name = "Product Model")]
        public Guid AdvProductModel { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string SearchTerm { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int? AdvReportType { get; set; }
        public int? AdvProgress { get; set; }
        [Display(Name = "Plant")]
        public int? AdvPlantCode { get; set; }
        public string PreparedBy { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public Guid AdvPreparedBy { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public PlantViewModel Plant { get; set; }
        public int? ReportType { get; set; }
        public decimal? ProdOrdQty { get; set; }
        public decimal? ProdQCQty { get; set; }
        public decimal? PendingQty { get; set; }
        public List<PendingProductionQCReportViewModel> PendingProductionQCReportList { get; set; }
    }

    public class QuotationDetailReportViewModel
    {
        public string QuotationNo { get; set; }
        public DateTime QuoteDate { get; set; }
        public string QuoteDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TaxableAmount { get; set; }
        public int? UnitCode { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Quotation From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Quotation To")]
        public string AdvToDate { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        [Display(Name = "Prepared By")]
        public Guid AdvPreparedBy { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        [Display(Name = "Total Amount (Incl.Tax) >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Total Amount (Incl.Tax) <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Product")]
        public string AdvProduct { get; set; }
        [Display(Name = "Product Model")]
        public string AdvProductModel { get; set; }
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
        [Display(Name = "Approval Status")]
        public int? AdvApprovalStatusCode { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "Report Type")]
        public int? AdvReportType { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<QuotationDetailReportViewModel> QuotationReportList { get; set; }
        public string QuoteNo { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }

    public class EnquiryDetailReportViewModel
    {
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Amount { get; set; }
        public int? UnitCode { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Enquiry From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Enquiry To")]
        public string AdvToDate { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        [Display(Name = "Attended By")]
        public Guid AdvAttendedBy { get; set; }
        [Display(Name = "Responsible Person")]
        public Guid AdvResponsibleBy { get; set; }
        [Display(Name = "Referred By")]
        public int? AdvReferencePersonCode { get; set; }
        public ReferencePersonViewModel ReferencePerson { get; set; }
        [Display(Name = "Amount >=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        [Display(Name = "Grade")]
        public int? AdvEnquiryGradeCode { get; set; }
        public EnquiryGradeViewModel EnquiryGrade { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Product")]
        public string AdvProduct { get; set; }
        [Display(Name = "Product Model")]
        public string AdvProductModel { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "Report Type")]
        public int? AdvReportType { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<EnquiryDetailReportViewModel> EnquiryReportList { get; set; }
        public string EnqNo { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
    }
    public class EstimateDetailReportViewModel
    {
        public string EstimateNo { get; set; }
        public DateTime EstimateDate { get; set; }
        public string EstimateDateFormatted { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public string ProductName { get; set; }
        public string ProdModel { get; set; }
        public decimal? Qty { get; set; }
        public decimal? TotalCostRate { get; set; }
        public decimal? TotalSellingRate { get; set; }
        public int? UnitCode { get; set; }
        public ProductModelViewModel ProductModel { get; set; }
        public UnitViewModel Unit { get; set; }
        public CustomerViewModel Customer { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Estimate From")]
        public string AdvFromDate { get; set; }
        [Display(Name = "Estimate To")]
        public string AdvToDate { get; set; }
        [Display(Name = "Estimate Valid From")]
        public string AdvValidFromDate { get; set; }
        [Display(Name = "Estimate Valid To")]
        public string AdvValidToDate { get; set; }
        [Display(Name = "Customer")]
        public string AdvCustomer { get; set; }
        [Display(Name = "Prepared By")]
        public Guid AdvPreparedBy { get; set; }
        [Display(Name = "Total Cost Rate >=")]
        public decimal? AdvTotalCostRateFrom { get; set; }
        [Display(Name = "Total Cost Rate <=")]
        public decimal? AdvTotalCostRateTo { get; set; }
        [Display(Name = "Total Selling Rate >=")]
        public decimal? AdvTotalSellingRateFrom { get; set; }
        [Display(Name = "Total Selling Rate <=")]
        public decimal? AdvTotalSellingRateTo { get; set; }
        [Display(Name = "District")]
        public int? AdvDistrictCode { get; set; }
        [Display(Name = "State")]
        public int? AdvStateCode { get; set; }
        [Display(Name = "Country")]
        public int? AdvCountryCode { get; set; }
        [Display(Name = "Area")]
        public int? AdvAreaCode { get; set; }
        [Display(Name = "Customer Category")]
        public int? AdvCustomerCategoryCode { get; set; }
        [Display(Name = "Branch")]
        public int? AdvBranchCode { get; set; }
        public BranchViewModel Branch { get; set; }
        [Display(Name = "Document Status")]
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatusViewModel DocumentStatus { get; set; }
        [Display(Name = "Document Owner")]
        public Guid AdvDocumentOwnerID { get; set; }
        public PSAUserViewModel PSAUser { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Product")]
        public string AdvProduct { get; set; }
        [Display(Name = "Product Model")]
        public string AdvProductModel { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "Report Type")]
        public int? AdvReportType { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public List<EstimateDetailReportViewModel> EstimateReportList { get; set; }
        public string EstNo { get; set; }
        public string DateFilter { get; set; }
        public PSASysCommonViewModel PSASysCommon { get; set; }
        public bool CostPriceHasAccess { get; set; }
    }
}
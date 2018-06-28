﻿using System;
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
        public Guid AdvCustomerID { get; set; }
        public CustomerViewModel Customer { get; set; }
        public int? AdvDistrictCode { get; set; }
        public int? AdvStateCode { get; set; }
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
        [Display(Name = "Amount <=")]
        public decimal? AdvAmountFrom { get; set; }
        [Display(Name = "Amount >=")]
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
}
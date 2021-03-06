﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Estimate
    {
        public Guid ID { get; set; }
        public string EstimateNo { get; set; }
        public string EstimateRefNo { get; set; }
        public DateTime EstimateDate { get; set; }
        public Guid? EnquiryID { get; set; }
        public Guid? CustomerID { get; set; }
        public int? DocumentStatusCode { get; set; }
        public DateTime ValidUpToDate { get; set; }
        public Guid? PreparedBy { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }

        //Aditional Fields
        public string DetailXML { get; set; }
        public string EstimateDateFormatted { get; set; }
        public string ValidUpToDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Enquiry Enquiry { get; set; }
        public Customer Customer { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Branch Branch { get; set; }
        public string UserName { get; set; }
        public List<EstimateDetail> EstimateDetailList { get; set; }
        public Employee Employee { get; set; }
        public Area Area { get; set; }
        public ReferencePerson ReferencePerson { get; set; }
        public Currency Currency { get; set; }
    }

    public class EstimateAdvanceSearch
    {
        public string EstimateDate { get; set; }
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

    public class EstimateDetail
    {
        public Guid ID { get; set; }
        public Guid EstimateID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? CostRate { get; set; }
        public decimal? SellingRate { get; set; }
        public string DrawingNo { get; set; }
        public Guid SpecTag { get; set; }

        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
    }
}

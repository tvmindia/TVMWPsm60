using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class ProformaInvoice
    {
        public Guid ID { get; set; }
        public string ProfInvNo { get; set; }
        public string ProfInvRefNo { get; set; }
        public DateTime ProfInvDate { get; set; }
        public Guid QuoteID { get; set; }
        public Guid SaleOrderID { get; set; }
        public Guid CustomerID { get; set; }
        public string MailingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int DocumentStatusCode { get; set; }
        public DateTime ExpectedDelvDate { get; set; }
        public bool CashInvoiceYN { get; set; }
        public Guid PreparedBy { get; set; }
        public string PurchaseOrdNo { get; set; }
        public DateTime PurchaseOrdDate { get; set; }
        public int BillSeriesCode { get; set; }
        public bool EmailSentYN { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public string EmailSentTo { get; set; }
        public string PrintRemark { get; set; }
        public string GeneralNotes { get; set; }
        public Guid DocumentOwnerID { get; set; }
        public int BranchCode { get; set; }
        public int? BillLocationCode { get; set; }
        public decimal Discount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }

        //additional Fields
        public string DetailXML { get; set; }
        public string OtherChargeDetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        public string ProfInvDateFormatted { get; set; }
        public bool IsUpdate { get; set; }
        public int FilteredCount { get; set; }
        public int TotalCount { get; set; }
        public string ExpectedDelvDateFormatted { get; set; }
        public string PurchaseOrdDateFormatted { get; set; }
        public string MailContant { get; set; }
        public string MailBodyFooter { get; set; }
        public string MailFrom { get; set; }
        public string SignatureStamp { get; set; }
        public string SignatureStampLine2 { get; set; }
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string CompanyAddress3 { get; set; }
        public string GSTIN { get; set; }
        public string CIN { get; set; }
        public string PAN { get; set; }
        public string EmailID { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public string DocumentType { get; set; }
        public string[] DocumentOwners { get; set; }
        public string DocumentOwner { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Customer Customer { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Branch Branch { get; set; }
        public string InvoiceType { get; set; }
        public BillLocation BillLocation { get; set; }
        public List<ProformaInvoiceOtherCharge> ProformaInvoiceOtherChargeDetailList { get; set; }
        public List<ProformaInvoiceDetail> ProformaInvoiceDetailList { get; set; }
        public Area Area { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public PSAUser PSAUser { get; set; }
        public string ReferenceNo { get; set; }
        public Quotation Quotation { get; set; }
        public SaleOrder SaleOrder { get; set; }
        public Currency Currency { get; set; }
    }

    public class ProformaInvoiceAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string AdvFromDate { get; set; }
        public string AdvToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid AdvCustomerID { get; set; }
        public int? AdvAreaCode { get; set; }
        public int? AdvBranchCode { get; set; }
        public int? AdvDocumentStatusCode { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Guid AdvDocumentOwnerID { get; set; }
        public int? AdvApprovalStatusCode { get; set; }
        public string AdvEmailSentStatus { get; set; }
    }

    public class ProformaInvoiceDetail
    {
        public Guid ID { get; set; }
        public Guid ProfInvID { get; set; }
        public Guid ProductID { get; set; }
        public Guid ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal Qty { get; set; }
        public int UnitCode { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public int TaxTypeCode { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
        public decimal CessPerc { get; set; }
        public decimal CessAmt { get; set; }
        public int? OtherChargeCode { get; set; }
        public Guid SpecTag { get; set; }
        public OtherCharge OtherCharge { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
        public TaxType TaxType { get; set; }
    }

    public class ProformaInvoiceOtherCharge
    {
        public Guid ID { get; set; }
        public Guid ProfInvID { get; set; }
        public int OtherChargeCode { get; set; }
        public decimal ChargeAmount { get; set; }
        public int TaxTypeCode { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
        public decimal AddlTaxPerc { get; set; }
        public decimal AddlTaxAmt { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public bool IsUpdate { get; set; }
        public OtherCharge OtherCharge { get; set; }
        public TaxType TaxType { get; set; }
    }
}

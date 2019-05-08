using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Service
{
    public class ProformaInvoiceRepository:IProformaInvoiceRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ProformaInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region Get All ProformaInvoice
        public List<ProformaInvoice> GetAllProformaInvoice(ProformaInvoiceAdvanceSearch proformaInvoiceAdvanceSearch)
        {
            List<ProformaInvoice> proformaInvoiceList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetAllProformaInvoice]";
                        if (string.IsNullOrEmpty(proformaInvoiceAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = proformaInvoiceAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = proformaInvoiceAdvanceSearch.DataTablePaging.Start;
                        if (proformaInvoiceAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = proformaInvoiceAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = proformaInvoiceAdvanceSearch.AdvFromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = proformaInvoiceAdvanceSearch.AdvToDate;
                        if (proformaInvoiceAdvanceSearch.AdvCustomerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = proformaInvoiceAdvanceSearch.AdvCustomerID;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = proformaInvoiceAdvanceSearch.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = proformaInvoiceAdvanceSearch.AdvBranchCode;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = proformaInvoiceAdvanceSearch.AdvDocumentStatusCode;
                        if (proformaInvoiceAdvanceSearch.AdvDocumentOwnerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@DocumentOwnerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = proformaInvoiceAdvanceSearch.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@ApprovalStatusCode", SqlDbType.Int).Value = proformaInvoiceAdvanceSearch.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.NVarChar).Value = proformaInvoiceAdvanceSearch.AdvEmailSentStatus;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                proformaInvoiceList = new List<ProformaInvoice>();
                                while (sdr.Read())
                                {
                                    ProformaInvoice proformaInvoice = new ProformaInvoice();
                                    {
                                        proformaInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : proformaInvoice.ID);
                                        proformaInvoice.ProfInvNo = (sdr["ProfInvNo"].ToString() != "" ? sdr["ProfInvNo"].ToString() : proformaInvoice.ProfInvNo);
                                        proformaInvoice.ProfInvRefNo = (sdr["ProfInvRefNo"].ToString() != "" ? sdr["ProfInvRefNo"].ToString() : proformaInvoice.ProfInvRefNo);
                                        proformaInvoice.ProfInvDate = (sdr["ProfInvDate"].ToString() != "" ? DateTime.Parse(sdr["ProfInvDate"].ToString()) : proformaInvoice.ProfInvDate);
                                        proformaInvoice.ProfInvDateFormatted = (sdr["ProfInvDate"].ToString() != "" ? DateTime.Parse(sdr["ProfInvDate"].ToString()).ToString(_settings.DateFormat) : proformaInvoice.ProfInvDateFormatted);
                                        proformaInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : proformaInvoice.CustomerID);
                                        proformaInvoice.Customer = new Customer();
                                        proformaInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : proformaInvoice.Customer.ID);
                                        proformaInvoice.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : proformaInvoice.Customer.CompanyName);
                                        proformaInvoice.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : proformaInvoice.Customer.ContactPerson);
                                        proformaInvoice.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : proformaInvoice.Customer.Mobile);
                                        proformaInvoice.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : proformaInvoice.DocumentStatusCode);
                                        proformaInvoice.DocumentStatus = new DocumentStatus();
                                        proformaInvoice.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : proformaInvoice.DocumentStatus.Code);
                                        proformaInvoice.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : proformaInvoice.DocumentStatus.Description);
                                        proformaInvoice.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : proformaInvoice.GeneralNotes);
                                        proformaInvoice.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : proformaInvoice.DocumentOwnerID);
                                        proformaInvoice.Branch = new Branch();
                                        proformaInvoice.Branch.Description = (sdr["BranchDescription"].ToString() != "" ? sdr["BranchDescription"].ToString() : proformaInvoice.Branch.Description);
                                        proformaInvoice.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : proformaInvoice.BranchCode);
                                        proformaInvoice.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : proformaInvoice.FilteredCount);
                                        proformaInvoice.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : proformaInvoice.FilteredCount);
                                        proformaInvoice.Area = new Area();
                                        proformaInvoice.Area.Description = (sdr["Area"].ToString() != "" ? sdr["Area"].ToString() : proformaInvoice.Area.Description);
                                        proformaInvoice.ApprovalStatus = new ApprovalStatus();
                                        proformaInvoice.ApprovalStatus.Description = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : proformaInvoice.ApprovalStatus.Description);
                                        proformaInvoice.PSAUser = new PSAUser();
                                        proformaInvoice.PSAUser.LoginName = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : proformaInvoice.PSAUser.LoginName);
                                        proformaInvoice.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : proformaInvoice.EmailSentYN);
                                        proformaInvoice.Quotation = new Quotation();
                                        proformaInvoice.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : proformaInvoice.QuoteID);
                                        proformaInvoice.Quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : proformaInvoice.Quotation.QuoteNo);
                                        proformaInvoice.SaleOrder = new SaleOrder();
                                        proformaInvoice.SaleOrderID= (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : proformaInvoice.SaleOrderID);
                                        proformaInvoice.SaleOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : proformaInvoice.SaleOrder.SaleOrderNo);
                                        proformaInvoice.InvoiceType = (sdr["InvocieType"].ToString() != "" ? (sdr["InvocieType"].ToString()) : proformaInvoice.InvoiceType);
                                    }
                                    proformaInvoiceList.Add(proformaInvoice);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return proformaInvoiceList;
        }
        #endregion Get All ProformaInvoice

        #region Get ProformaInvoice
        public ProformaInvoice GetProformaInvoice(Guid id)
        {
            ProformaInvoice proformaInvoice = new ProformaInvoice();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetProformaInvoice]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    proformaInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : proformaInvoice.ID);
                                    proformaInvoice.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : proformaInvoice.QuoteID);
                                    proformaInvoice.SaleOrderID = (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : proformaInvoice.SaleOrderID);
                                    proformaInvoice.ProfInvNo = (sdr["ProfInvNo"].ToString() != "" ? sdr["ProfInvNo"].ToString() : proformaInvoice.ProfInvNo);
                                    proformaInvoice.ProfInvRefNo = (sdr["ProfInvRefNo"].ToString() != "" ? sdr["ProfInvRefNo"].ToString() : proformaInvoice.ProfInvRefNo);
                                    proformaInvoice.InvoiceType = (sdr["InvocieType"].ToString() != "" ? sdr["InvocieType"].ToString() : proformaInvoice.InvoiceType);
                                    proformaInvoice.ProfInvDate = (sdr["ProfInvDate"].ToString() != "" ? DateTime.Parse(sdr["ProfInvDate"].ToString()) : proformaInvoice.ProfInvDate);
                                    proformaInvoice.ProfInvDateFormatted = (sdr["ProfInvDate"].ToString() != "" ? DateTime.Parse(sdr["ProfInvDate"].ToString()).ToString("dd-MMM-yyyy") : proformaInvoice.ProfInvDateFormatted);
                                    proformaInvoice.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : proformaInvoice.ExpectedDelvDate);
                                    proformaInvoice.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString("dd-MMM-yyyy") : proformaInvoice.ExpectedDelvDateFormatted);
                                    proformaInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : proformaInvoice.CustomerID);
                                    proformaInvoice.Customer = new Customer();
                                    proformaInvoice.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : proformaInvoice.Customer.CompanyName);
                                    proformaInvoice.Customer.ID= (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : proformaInvoice.Customer.ID);
                                    proformaInvoice.Customer.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : proformaInvoice.Customer.TaxRegNo);
                                    proformaInvoice.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : proformaInvoice.DocumentStatusCode);
                                    proformaInvoice.DocumentStatus = new DocumentStatus();
                                    proformaInvoice.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? sdr["DocumentStatusDescription"].ToString() : proformaInvoice.DocumentStatus.Description);
                                    proformaInvoice.MailingAddress = (sdr["MailingAddress"].ToString() != "" ? sdr["MailingAddress"].ToString() : proformaInvoice.MailingAddress);
                                    proformaInvoice.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : proformaInvoice.ShippingAddress);
                                    proformaInvoice.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : proformaInvoice.GeneralNotes);
                                    proformaInvoice.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : proformaInvoice.DocumentOwnerID);
                                    proformaInvoice.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : proformaInvoice.BranchCode);
                                    proformaInvoice.BillLocationCode = (sdr["BillingLocationCode"].ToString() != "" ? int.Parse(sdr["BillingLocationCode"].ToString()) : proformaInvoice.BillLocationCode);
                                    proformaInvoice.BillLocation = new BillLocation();
                                    proformaInvoice.BillLocation.Address = (sdr["BillingLocationAddress"].ToString() != "" ? sdr["BillingLocationAddress"].ToString() : proformaInvoice.BillLocation.Address);
                                    proformaInvoice.BillLocation.Name = (sdr["BillingLocationName"].ToString() != "" ? sdr["BillingLocationName"].ToString() : proformaInvoice.BillLocation.Name);
                                    proformaInvoice.Branch = new Branch();
                                    proformaInvoice.Branch.Code= (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : proformaInvoice.Branch.Code);
                                    proformaInvoice.Branch.Description = (sdr["BranchDescription"].ToString() != "" ? sdr["BranchDescription"].ToString() : proformaInvoice.Branch.Description);
                                    proformaInvoice.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : proformaInvoice.PreparedBy);
                                    proformaInvoice.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : proformaInvoice.Discount);
                                    proformaInvoice.DocumentOwners = (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : proformaInvoice.DocumentOwners);
                                    proformaInvoice.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : proformaInvoice.DocumentOwner);
                                    string mailfooter = (sdr["MailBodyFooter"].ToString() != "" ? (sdr["MailBodyFooter"].ToString()) : proformaInvoice.MailBodyFooter);
                                    proformaInvoice.MailBodyFooter = mailfooter.Replace("\n", "<br />");
                                    string mailfrom = (sdr["MailFromAddress"].ToString() != "" ? (sdr["MailFromAddress"].ToString()) : proformaInvoice.MailFrom);
                                    proformaInvoice.MailFrom = mailfrom.Replace("\n", "<br />");
                                    proformaInvoice.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : proformaInvoice.EmailSentYN);
                                    proformaInvoice.EmailSentTo = (sdr["EmailSentTo"].ToString() != "" ? sdr["EmailSentTo"].ToString() : proformaInvoice.EmailSentTo);
                                    proformaInvoice.Cc = (sdr["Cc"].ToString() != "" ? (sdr["Cc"].ToString()) : proformaInvoice.Cc);
                                    proformaInvoice.Bcc = (sdr["Bcc"].ToString() != "" ? (sdr["Bcc"].ToString()) : proformaInvoice.Bcc);
                                    proformaInvoice.Subject = (sdr["Subject"].ToString() != "" ? (sdr["Subject"].ToString()) : proformaInvoice.Subject);
                                    proformaInvoice.GSTIN = (sdr["GSTIN"].ToString() != "" ? sdr["GSTIN"].ToString() : proformaInvoice.GSTIN);
                                    proformaInvoice.CIN = (sdr["CIN"].ToString() != "" ? sdr["CIN"].ToString() : proformaInvoice.CIN);
                                    proformaInvoice.EmailID = (sdr["EmailID"].ToString() != "" ? sdr["EmailID"].ToString() : proformaInvoice.EmailID);
                                    proformaInvoice.PAN = (sdr["PAN"].ToString() != "" ? sdr["PAN"].ToString() : proformaInvoice.PAN);
                                    proformaInvoice.SignatureStamp = (sdr["SignatureStamp"].ToString() != "" ? sdr["SignatureStamp"].ToString() : proformaInvoice.SignatureStamp);
                                    proformaInvoice.SignatureStampLine2 = (sdr["SignatureStampLine2"].ToString() != "" ? sdr["SignatureStampLine2"].ToString() : proformaInvoice.SignatureStampLine2);
                                    string CompanyAddress1 = (sdr["CompanyAddress1"].ToString() != "" ? sdr["CompanyAddress1"].ToString() : proformaInvoice.CompanyAddress1);
                                    proformaInvoice.CompanyAddress1 = CompanyAddress1.Replace("\\n", "<br />");
                                    string CompanyAddress2 = (sdr["CompanyAddress2"].ToString() != "" ? sdr["CompanyAddress2"].ToString() : proformaInvoice.CompanyAddress2);
                                    proformaInvoice.CompanyAddress2 = CompanyAddress2.Replace("\\n", "<br />");
                                    string CompanyAddress3 = (sdr["CompanyAddress3"].ToString() != "" ? sdr["CompanyAddress3"].ToString() : proformaInvoice.CompanyAddress3);
                                    proformaInvoice.CompanyAddress3 = CompanyAddress3.Replace("\\n", "<br />");
                                    proformaInvoice.PurchaseOrdNo = (sdr["PurchaseOrdNo"].ToString() != "" ? sdr["PurchaseOrdNo"].ToString() : proformaInvoice.PurchaseOrdNo);
                                    proformaInvoice.Quotation = new Quotation();
                                    proformaInvoice.Quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : proformaInvoice.Quotation.QuoteNo);
                                    proformaInvoice.SaleOrder = new SaleOrder();
                                    proformaInvoice.SaleOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : proformaInvoice.SaleOrder.SaleOrderNo);
                                    proformaInvoice.CurrencyCode = (sdr["CurrencyCode"].ToString() != "" ? sdr["CurrencyCode"].ToString() : proformaInvoice.CurrencyCode);
                                    proformaInvoice.CurrencyRate = (sdr["CurrencyRate"].ToString() != "" ? Decimal.Parse(sdr["CurrencyRate"].ToString()) : proformaInvoice.CurrencyRate);
                                    proformaInvoice.Currency = new Currency();
                                    proformaInvoice.Currency.Description = (sdr["CurrencyDescription"].ToString() != "" ? sdr["CurrencyDescription"].ToString() : proformaInvoice.Currency.Description);
                                    proformaInvoice.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : proformaInvoice.LatestApprovalID);
                                    proformaInvoice.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : proformaInvoice.LatestApprovalStatus);
                                    proformaInvoice.LatestApprovalStatusDescription = (sdr["ApprovalDescription"].ToString() != "" ? (sdr["ApprovalDescription"].ToString()) : proformaInvoice.LatestApprovalStatusDescription);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return proformaInvoice;
        }
        #endregion Get ProformaInvoice

        #region GetAllProformaInvoiceItems
        public List<ProformaInvoiceDetail> GetProformaInvoiceDetailListByProformaInvoiceID(Guid proformaInvoiceID)
        {
            List<ProformaInvoiceDetail> proformaInvoiceDetailList = new List<ProformaInvoiceDetail>();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetProformaInvoiceDetailByProformaInvoiceID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ProformaInvoiceID", SqlDbType.UniqueIdentifier).Value = proformaInvoiceID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    ProformaInvoiceDetail proformaInvoiceDetail = new ProformaInvoiceDetail();
                                    {
                                        proformaInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : proformaInvoiceDetail.ID);
                                        proformaInvoiceDetail.ProfInvID = (sdr["ProfInvID"].ToString() != "" ? Guid.Parse(sdr["ProfInvID"].ToString()) : proformaInvoiceDetail.ProfInvID);
                                        proformaInvoiceDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : proformaInvoiceDetail.ProductSpec);
                                        proformaInvoiceDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty),
                                            HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : String.Empty)
                                        };
                                        proformaInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        proformaInvoiceDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        proformaInvoiceDetail.ProductModel = new ProductModel();
                                        proformaInvoiceDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        proformaInvoiceDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : proformaInvoiceDetail.ProductModel.Name);

                                        proformaInvoiceDetail.OtherCharge = new OtherCharge();
                                        proformaInvoiceDetail.OtherCharge.Code = (sdr["OtherChargeCode"].ToString() != "" ? int.Parse(sdr["OtherChargeCode"].ToString()) : proformaInvoiceDetail.OtherCharge.Code);
                                        proformaInvoiceDetail.OtherCharge.Description = (sdr["OtherChargeCodeDesc"].ToString() != "" ? (sdr["OtherChargeCodeDesc"].ToString()) : proformaInvoiceDetail.OtherCharge.Description);
                                        proformaInvoiceDetail.OtherChargeCode = (sdr["OtherChargeCode"].ToString() != "" ? int.Parse(sdr["OtherChargeCode"].ToString()) : proformaInvoiceDetail.OtherChargeCode);
                                        proformaInvoiceDetail.OtherCharge.SACCode = (sdr["SACCode"].ToString() != "" ? sdr["SACCode"].ToString() : proformaInvoiceDetail.OtherCharge.SACCode);
                                        proformaInvoiceDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : proformaInvoiceDetail.Qty);
                                        proformaInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : proformaInvoiceDetail.Rate);
                                        proformaInvoiceDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : proformaInvoiceDetail.UnitCode);
                                        proformaInvoiceDetail.Unit = new Unit();
                                        proformaInvoiceDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : proformaInvoiceDetail.Unit.Code);
                                        proformaInvoiceDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : proformaInvoiceDetail.Unit.Description);
                                        proformaInvoiceDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : proformaInvoiceDetail.CGSTPerc);
                                        proformaInvoiceDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : proformaInvoiceDetail.IGSTPerc);
                                        proformaInvoiceDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : proformaInvoiceDetail.SGSTPerc);
                                        proformaInvoiceDetail.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : proformaInvoiceDetail.Discount);
                                        proformaInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : proformaInvoiceDetail.TaxTypeCode);
                                        proformaInvoiceDetail.TaxType = new TaxType();
                                        proformaInvoiceDetail.TaxType.ValueText = (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : proformaInvoiceDetail.TaxType.ValueText);
                                        proformaInvoiceDetail.CessAmt = (sdr["CessAmt"].ToString() != "" ? decimal.Parse(sdr["CessAmt"].ToString()) : proformaInvoiceDetail.CessAmt);
                                        proformaInvoiceDetail.CessPerc = (sdr["CessPerc"].ToString() != "" ? decimal.Parse(sdr["CessPerc"].ToString()) : proformaInvoiceDetail.CessPerc);
                                    }
                                    proformaInvoiceDetailList.Add(proformaInvoiceDetail);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return proformaInvoiceDetailList;
        }


        #endregion GetAllProformaInvoiceItems

        #region Insert Update ProformaInvoice
        public object InsertUpdateProformaInvoice(ProformaInvoice proformaInvoice)
        {
            SqlParameter outputStatus, outputID, outputProdOrderNo = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[InsertUpdateProformaInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = proformaInvoice.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.ID;
                        cmd.Parameters.Add("@ProfInvNo", SqlDbType.VarChar, 20).Value = proformaInvoice.ProfInvNo;
                        cmd.Parameters.Add("@ProfInvRefNo", SqlDbType.VarChar, 20).Value = proformaInvoice.ProfInvRefNo;
                        cmd.Parameters.Add("@ProfInvDate", SqlDbType.DateTime).Value = proformaInvoice.ProfInvDateFormatted;
                        if (proformaInvoice.QuoteID != Guid.Empty)
                            cmd.Parameters.Add("@QuoteID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.QuoteID;
                        if (proformaInvoice.SaleOrderID != Guid.Empty)
                            cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.SaleOrderID;
                        if (proformaInvoice.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.CustomerID;
                        cmd.Parameters.Add("@MailingAddress", SqlDbType.NVarChar, -1).Value = proformaInvoice.MailingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = proformaInvoice.ShippingAddress;
                       // cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = proformaInvoice.DocumentStatusCode;
                        cmd.Parameters.Add("@BillingLocationCode", SqlDbType.Int).Value = proformaInvoice.BillLocationCode;
                        cmd.Parameters.Add("@ExpectedDelvDate", SqlDbType.DateTime).Value = proformaInvoice.ExpectedDelvDateFormatted;
                        cmd.Parameters.Add("@CashInvoiceYN", SqlDbType.Bit).Value = proformaInvoice.CashInvoiceYN;
                        if (proformaInvoice.PreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = proformaInvoice.PreparedBy;
                        cmd.Parameters.Add("@PurchaseOrdNo", SqlDbType.VarChar, 20).Value = proformaInvoice.PurchaseOrdNo;
                        cmd.Parameters.Add("@InvocieType", SqlDbType.VarChar, 20).Value = proformaInvoice.InvoiceType;
                        cmd.Parameters.Add("@PurchaseOrdDate", SqlDbType.DateTime).Value = proformaInvoice.PurchaseOrdDateFormatted;
                        cmd.Parameters.Add("@BillSeriesCode", SqlDbType.Int).Value = proformaInvoice.BillSeriesCode;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = proformaInvoice.EmailSentYN;
                        cmd.Parameters.Add("@LatestApprovalID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.LatestApprovalID;
                        cmd.Parameters.Add("@LatestApprovalStatus", SqlDbType.Int).Value = proformaInvoice.LatestApprovalStatus;
                        cmd.Parameters.Add("@IsFinalApproved", SqlDbType.Bit).Value = proformaInvoice.IsFinalApproved;
                        cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar, -1).Value = proformaInvoice.EmailSentTo;
                        cmd.Parameters.Add("@PrintRemark", SqlDbType.NVarChar, -1).Value = proformaInvoice.PrintRemark;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = proformaInvoice.Discount;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = proformaInvoice.AdvanceAmount;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = proformaInvoice.DetailXML;
                        cmd.Parameters.Add("@OtherChargeDetailXML", SqlDbType.Xml).Value = proformaInvoice.OtherChargeDetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = proformaInvoice.GeneralNotes;
                        if (proformaInvoice.DocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = proformaInvoice.BranchCode;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = proformaInvoice.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = proformaInvoice.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = proformaInvoice.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = proformaInvoice.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@CurrencyCode", SqlDbType.VarChar).Value = proformaInvoice.CurrencyCode;
                        cmd.Parameters.Add("@CurrencyRate", SqlDbType.Decimal).Value = proformaInvoice.CurrencyRate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputProdOrderNo = cmd.Parameters.Add("@ProfInvNoOut", SqlDbType.VarChar, 20);
                        outputProdOrderNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        proformaInvoice.ID = Guid.Parse(outputID.Value.ToString());
                        proformaInvoice.ProfInvNo = outputProdOrderNo.Value.ToString();
                        return new
                        {
                            ID = proformaInvoice.ID,
                            ProformaInvoiceNo = proformaInvoice.ProfInvNo,
                            Status = outputStatus.Value.ToString(),
                            Message = proformaInvoice.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = proformaInvoice.ID,
                ProformaInvoiceNo = proformaInvoice.ProfInvNo,
                Status = outputStatus.Value.ToString(),
                Message = proformaInvoice.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update ProformaInvoice

        #region Delete ProformaInvoice
        public object DeleteProformaInvoice(Guid id)
        {
            SqlParameter outputStatus = null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[DeleteProformaInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConstant.DeleteFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion Delete ProformaInvoice

        #region Delete ProformaInvoice Detail
        public object DeleteProformaInvoiceDetail(Guid id, string CreatedBy, DateTime CreatedDate)
        {
            SqlParameter outputStatus = null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[DeleteProformaInvoiceDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConstant.DeleteFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion Delete ProformaInvoice Detail

        #region Get Proforma Invoice OtherCharge Detail
        public List<ProformaInvoiceOtherCharge> GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(Guid proformaInvoiceID)
        {
            List<ProformaInvoiceOtherCharge> proformaInvoiceOtherChargeList = new List<ProformaInvoiceOtherCharge>();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetProformaInvoiceOtherChargeListByProformaInvoiceID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ProformaInvoiceID", SqlDbType.UniqueIdentifier).Value = proformaInvoiceID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    ProformaInvoiceOtherCharge proformaInvoiceOtherCharge = new ProformaInvoiceOtherCharge();
                                    {
                                        proformaInvoiceOtherCharge.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : proformaInvoiceOtherCharge.ID);
                                        proformaInvoiceOtherCharge.ProfInvID = (sdr["ProfInvID"].ToString() != "" ? Guid.Parse(sdr["ProfInvID"].ToString()) : proformaInvoiceOtherCharge.ProfInvID);
                                        proformaInvoiceOtherCharge.OtherChargeCode = (sdr["OtherChargeCode"].ToString() != "" ? int.Parse(sdr["OtherChargeCode"].ToString()) : proformaInvoiceOtherCharge.OtherChargeCode);
                                        proformaInvoiceOtherCharge.ChargeAmount = (sdr["ChargeAmount"].ToString() != "" ? decimal.Parse(sdr["ChargeAmount"].ToString()) : proformaInvoiceOtherCharge.ChargeAmount);
                                        proformaInvoiceOtherCharge.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : proformaInvoiceOtherCharge.TaxTypeCode);
                                        proformaInvoiceOtherCharge.TaxType = new TaxType();
                                        proformaInvoiceOtherCharge.TaxType.Code = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : proformaInvoiceOtherCharge.TaxType.Code);
                                        proformaInvoiceOtherCharge.TaxType.ValueText = (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : proformaInvoiceOtherCharge.TaxType.ValueText);
                                        proformaInvoiceOtherCharge.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : proformaInvoiceOtherCharge.CGSTPerc);
                                        proformaInvoiceOtherCharge.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : proformaInvoiceOtherCharge.SGSTPerc);
                                        proformaInvoiceOtherCharge.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : proformaInvoiceOtherCharge.IGSTPerc);
                                        proformaInvoiceOtherCharge.AddlTaxPerc = (sdr["AddlTaxPerc"].ToString() != "" ? decimal.Parse(sdr["AddlTaxPerc"].ToString()) : proformaInvoiceOtherCharge.AddlTaxPerc);
                                        proformaInvoiceOtherCharge.AddlTaxAmt = (sdr["AddlTaxAmt"].ToString() != "" ? decimal.Parse(sdr["AddlTaxAmt"].ToString()) : proformaInvoiceOtherCharge.AddlTaxAmt);
                                        proformaInvoiceOtherCharge.OtherCharge = new OtherCharge();
                                        proformaInvoiceOtherCharge.OtherCharge.Description = (sdr["OtherCharge"].ToString() != "" ? sdr["OtherCharge"].ToString() : proformaInvoiceOtherCharge.OtherCharge.Description);
                                        proformaInvoiceOtherCharge.OtherCharge.SACCode= (sdr["SACCode"].ToString() != "" ? sdr["SACCode"].ToString() : proformaInvoiceOtherCharge.OtherCharge.SACCode);
                                    }
                                    proformaInvoiceOtherChargeList.Add(proformaInvoiceOtherCharge);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proformaInvoiceOtherChargeList;
        }
        #endregion Get Proforma Invoice OtherCharge Detail

        #region Update Proforma Invoice Email Info
        public object UpdateProformaInvoiceEmailInfo(ProformaInvoice proformaInvoice)
        {
            SqlParameter outputStatus = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[UpdateProformaInvoiceEmailInfo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = proformaInvoice.ID;
                       
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = proformaInvoice.EmailSentYN;
                        cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar, -1).Value = proformaInvoice.EmailSentTo;
                        cmd.Parameters.Add("@Cc", SqlDbType.NVarChar, -1).Value = proformaInvoice.Cc;
                        cmd.Parameters.Add("@Bcc", SqlDbType.NVarChar, -1).Value = proformaInvoice.Bcc;
                        cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, -1).Value = proformaInvoice.Subject;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = proformaInvoice.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = proformaInvoice.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = proformaInvoice.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = proformaInvoice.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Update Proforma Invoice Email Info

        #region Delete Proforma Invoice OtherCharge Detail
        public object DeleteProformaInvoiceOtherChargeDetail(Guid id, string CreatedBy, DateTime CreatedDate)
        {
            SqlParameter outputStatus = null;
            try
            {

                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[DeleteProformaInvoiceOtherCharge]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.DeleteFailure);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion Delete Proforma Invoice OtherCharge Detail

        #region GetProformaInvoiceForSelectListOnDemand
        public List<ProformaInvoice> GetProformaInvoiceForSelectListOnDemand(string searchTerm)
        {
            List<ProformaInvoice> proformaInvoiceList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetProformaInvoiceForSelectListOnDemand]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (string.IsNullOrEmpty(searchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 250).Value = searchTerm;
                        }
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                proformaInvoiceList = new List<ProformaInvoice>();
                                while (sdr.Read())
                                {
                                    ProformaInvoice proformaInvoice = new ProformaInvoice();
                                    {
                                        proformaInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : proformaInvoice.ID);
                                        proformaInvoice.ProfInvNo = (sdr["ProfInvNo"].ToString() != "" ? sdr["ProfInvNo"].ToString() : proformaInvoice.ProfInvNo);
                                    }
                                    proformaInvoiceList.Add(proformaInvoice);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proformaInvoiceList;
        }
        #endregion GetProformaInvoiceForSelectListOnDemand

        #region GetProformaInvoiceForSelectList
        public List<ProformaInvoice> GetProformaInvoiceForSelectList(Guid? proformaInvoiceID)
        {
            List<ProformaInvoice> proformaInvoiceList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetProformaInvoiceForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (proformaInvoiceID!=null)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.VarChar, 250).Value = proformaInvoiceID;
                        }
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                proformaInvoiceList = new List<ProformaInvoice>();
                                while (sdr.Read())
                                {
                                    ProformaInvoice proformaInvoice = new ProformaInvoice();
                                    {
                                        proformaInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : proformaInvoice.ID);
                                        proformaInvoice.ProfInvNo = (sdr["ProfInvNo"].ToString() != "" ? sdr["ProfInvNo"].ToString() : proformaInvoice.ProfInvNo);
                                    }
                                    proformaInvoiceList.Add(proformaInvoice);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proformaInvoiceList;
        }
        #endregion GetProformaInvoiceForSelectList

    }
}

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
    public class QuotationRepository: IQuotationRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public QuotationRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get All Quotation
        public List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch)
        {
            List<Quotation> quotationList = null;
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
                        cmd.CommandText = "[PSA].[GetAllQuotation]";
                        if (string.IsNullOrEmpty(quotationAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = quotationAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = quotationAdvanceSearch.DataTablePaging.Start;
                        if (quotationAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = quotationAdvanceSearch.DataTablePaging.Length;                        
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = quotationAdvanceSearch.AdvFromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = quotationAdvanceSearch.AdvToDate;
                        if (quotationAdvanceSearch.AdvCustomerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = quotationAdvanceSearch.AdvCustomerID;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = quotationAdvanceSearch.AdvAreaCode;
                        cmd.Parameters.Add("@ReferencePersonCode", SqlDbType.Int).Value = quotationAdvanceSearch.AdvReferencePersonCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = quotationAdvanceSearch.AdvBranchCode;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = quotationAdvanceSearch.AdvDocumentStatusCode;
                        if (quotationAdvanceSearch.AdvDocumentOwnerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@DocumentOwnerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = quotationAdvanceSearch.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@ApprovalStatusCode", SqlDbType.Int).Value = quotationAdvanceSearch.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.NVarChar,5).Value = quotationAdvanceSearch.AdvEmailSentStatus;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                quotationList = new List<Quotation>();
                                while (sdr.Read())
                                {
                                    Quotation quotation = new Quotation();
                                    {
                                        quotation.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotation.ID);
                                        quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : quotation.QuoteNo);
                                        quotation.QuoteDate = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()) : quotation.QuoteDate);
                                        quotation.QuoteDateFormatted = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()).ToString(_settings.DateFormat) : quotation.QuoteDateFormatted);
                                        //quotation.RequirementSpec = (sdr["RequirementSpec"].ToString() != "" ? sdr["RequirementSpec"].ToString() : quotation.RequirementSpec);
                                        quotation.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : quotation.CustomerID);
                                        quotation.Customer = new Customer();
                                        quotation.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : quotation.Customer.ID);
                                        quotation.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : quotation.Customer.CompanyName);
                                        quotation.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : quotation.Customer.ContactPerson);
                                        quotation.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : quotation.Customer.Mobile);
                                        quotation.ValidUpToDate = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()) : quotation.ValidUpToDate);
                                        quotation.ValidUpToDateFormatted = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()).ToString(_settings.DateFormat) : quotation.ValidUpToDateFormatted);
                                        quotation.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : quotation.DocumentStatusCode);
                                        quotation.DocumentStatus = new DocumentStatus();
                                        quotation.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : quotation.DocumentStatus.Code);
                                        quotation.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : quotation.DocumentStatus.Description);
                                        quotation.IsFinalApproved = (sdr["IsFinalApproved"].ToString() != "" ? bool.Parse(sdr["IsFinalApproved"].ToString()) : quotation.IsFinalApproved);
                                        quotation.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : quotation.ReferredByCode);
                                        quotation.ReferencePerson = new ReferencePerson();
                                        quotation.ReferencePerson.Code = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : quotation.ReferencePerson.Code);
                                        quotation.ReferencePerson.Name = (sdr["ReferredByCode"].ToString() != "" ? (sdr["ReferencePersonName"].ToString()) : quotation.ReferencePerson.Name);
                                        //quotation.ResponsiblePersonID = (sdr["ReferencePersonName"].ToString() != "" ? Guid.Parse(sdr["ResponsiblePersonID"].ToString()) : quotation.ResponsiblePersonID);
                                        quotation.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : quotation.PreparedBy);
                                        //quotation.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : quotation.GeneralNotes);
                                        quotation.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : quotation.DocumentOwnerID);
                                        quotation.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : quotation.BranchCode);
                                        quotation.Branch = new Branch();
                                        quotation.Branch.Description = (sdr["Branch"].ToString() != "" ? (sdr["Branch"].ToString()) : quotation.Branch.Description);
                                        quotation.EstimateID= (sdr["EstimateID"].ToString() != "" ? Guid.Parse(sdr["EstimateID"].ToString()) : quotation.EstimateID);
                                        quotation.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : quotation.FilteredCount);
                                        quotation.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : quotation.FilteredCount);
                                        quotation.Area = new Area();
                                        quotation.Area.Description = (sdr["Area"].ToString() != "" ? (sdr["Area"].ToString()) : quotation.Area.Description);
                                        quotation.PSAUser = new PSAUser();
                                        quotation.PSAUser.LoginName = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : quotation.PSAUser.LoginName);
                                        quotation.ApprovalStatus = new ApprovalStatus();
                                        quotation.ApprovalStatus.Code= (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : quotation.ApprovalStatus.Code);
                                        quotation.ApprovalStatus.Description = (sdr["ApprovalStatus"].ToString() != "" ? (sdr["ApprovalStatus"].ToString()) : quotation.ApprovalStatus.Description);
                                        quotation.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse (sdr["EmailSentYN"].ToString()) : quotation.EmailSentYN);
                                        quotation.EstimateNo = (sdr["EstimateNo"].ToString() != "" ? sdr["EstimateNo"].ToString() : quotation.EstimateNo);
                                    }
                                    quotationList.Add(quotation);
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

            return quotationList;
        }
        #endregion Get All Quotation
        #region Get Quotation
        public Quotation GetQuotation(Guid id)
        {
            Quotation quotation = new Quotation();
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
                        cmd.CommandText = "[PSA].[GetQuotation]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    quotation.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotation.ID);
                                    quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : quotation.QuoteNo);
                                    quotation.QuoteRefNo = (sdr["QuoteRefNo"].ToString() != "" ? sdr["QuoteRefNo"].ToString() : quotation.QuoteRefNo);
                                    quotation.QuoteDate = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()) : quotation.QuoteDate);
                                    quotation.QuoteDateFormatted = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()).ToString("dd-MMM-yyyy") : quotation.QuoteDateFormatted);
                                    quotation.EstimateID = (sdr["EstimateID"].ToString() != "" ? Guid.Parse(sdr["EstimateID"].ToString()) : quotation.EstimateID);
                                    quotation.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : quotation.CustomerID);
                                    quotation.Customer = new Customer();
                                    quotation.Customer.CompanyName= (sdr["CustomerCompanyName"].ToString() != "" ? (sdr["CustomerCompanyName"].ToString()) : quotation.Customer.CompanyName);
                                    quotation.Customer.ID= (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : quotation.Customer.ID);
                                    quotation.MailingAddress = (sdr["MailingAddress"].ToString() != "" ? sdr["MailingAddress"].ToString() : quotation.MailingAddress);
                                    quotation.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : quotation.ShippingAddress);
                                    quotation.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : quotation.DocumentStatusCode);
                                    quotation.DocumentStatus = new DocumentStatus();
                                    quotation.DocumentStatus.Description = (sdr["DocumentStatus"].ToString() != "" ? sdr["DocumentStatus"].ToString() : quotation.DocumentStatus.Description);
                                    quotation.ValidUpToDate = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()) : quotation.ValidUpToDate);
                                    quotation.ValidUpToDateFormatted = (sdr["ValidUpToDate"].ToString() != "" ? DateTime.Parse(sdr["ValidUpToDate"].ToString()).ToString("dd-MMM-yyyy") : quotation.ValidUpToDateFormatted);
                                    quotation.ReferredByCode = (sdr["ReferredByCode"].ToString() != "" ? int.Parse(sdr["ReferredByCode"].ToString()) : quotation.ReferredByCode);
                                    quotation.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : quotation.PreparedBy);
                                    quotation.MailBodyHeader = (sdr["MailBodyHeader"].ToString() != "" ? sdr["MailBodyHeader"].ToString() : quotation.MailBodyHeader);
                                    quotation.MailBodyFooter = (sdr["MailBodyFooter"].ToString() != "" ? sdr["MailBodyFooter"].ToString() : quotation.MailBodyFooter);
                                    quotation.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : quotation.EmailSentYN);
                                    quotation.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : quotation.LatestApprovalID);
                                    quotation.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : quotation.LatestApprovalStatus);
                                    quotation.LatestApprovalStatusDescription= (sdr["ApprovalDescription"].ToString() != "" ? (sdr["ApprovalDescription"].ToString()) : quotation.LatestApprovalStatusDescription);
                                    quotation.IsFinalApproved = (sdr["IsFinalApproved"].ToString() != "" ? bool.Parse(sdr["IsFinalApproved"].ToString()) : quotation.IsFinalApproved);
                                    quotation.EmailSentTo = (sdr["EmailSentTo"].ToString() != "" ? (sdr["EmailSentTo"].ToString()) : quotation.EmailSentTo);
                                    quotation.Cc = (sdr["Cc"].ToString() != "" ? (sdr["Cc"].ToString()) : quotation.Cc);
                                    quotation.Bcc = (sdr["Bcc"].ToString() != "" ? (sdr["Bcc"].ToString()) : quotation.Bcc);
                                    quotation.Subject = (sdr["Subject"].ToString() != "" ? (sdr["Subject"].ToString()) : quotation.Subject);
                                    quotation.TermReferenceNo = (sdr["TermReferenceNo"].ToString() != "" ? (sdr["TermReferenceNo"].ToString()) : quotation.TermReferenceNo);
                                    quotation.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : quotation.Discount);
                                    quotation.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : quotation.GeneralNotes);
                                    quotation.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : quotation.DocumentOwnerID);
                                    quotation.DocumentOwners = (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : quotation.DocumentOwners);
                                    quotation.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : quotation.DocumentOwner);
                                    quotation.Branch = new Branch();
                                    quotation.Branch.Description= (sdr["Branch"].ToString() != "" ? sdr["Branch"].ToString() : quotation.Branch.Description);
                                    quotation.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : quotation.BranchCode);
                                    quotation.Branch.Code= (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : quotation.Branch.Code);
                                    quotation.ApproverLevel = (sdr["ApproverLevel"].ToString() != "" ? int.Parse(sdr["ApproverLevel"].ToString()) : quotation.ApproverLevel);                                                                   
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return quotation;
        }
        #endregion Get Quotation
        #region GetQuotationDetailListByQuotationID
        public List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quotationID)
        {
            List<QuotationDetail> quotationDetailList = new List<QuotationDetail>();
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
                        cmd.CommandText = "[PSA].[GetQuotationDetailListByQuotationID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@QuoteID", SqlDbType.UniqueIdentifier).Value = quotationID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    QuotationDetail quotationDetail = new QuotationDetail();
                                    {
                                        quotationDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotationDetail.ID);
                                        quotationDetail.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : quotationDetail.QuoteID);
                                        quotationDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : quotationDetail.ProductSpec);
                                        quotationDetail.ProductSpecHtml = (sdr["ProductSpecHtml"].ToString() != "" ? sdr["ProductSpecHtml"].ToString() : quotationDetail.ProductSpecHtml);
                                        quotationDetail.SpecTag = (sdr["SpecTag"].ToString() != "" ? Guid.Parse(sdr["SpecTag"].ToString()) : quotationDetail.SpecTag);
                                        quotationDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty),
                                            HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : String.Empty)
                                        };
                                        quotationDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        quotationDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        quotationDetail.ProductModel = new ProductModel();
                                        quotationDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        quotationDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : quotationDetail.ProductModel.Name);
                                        quotationDetail.ProductModel.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : quotationDetail.ProductModel.ImageURL);
                                        quotationDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : quotationDetail.Qty);
                                        quotationDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : quotationDetail.Rate);
                                        quotationDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : quotationDetail.UnitCode);
                                        quotationDetail.Unit = new Unit();
                                        quotationDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : quotationDetail.Unit.Code);
                                        quotationDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : quotationDetail.Unit.Description);
                                        quotationDetail.Discount= (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : quotationDetail.Discount);
                                        quotationDetail.TaxTypeCode= (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : quotationDetail.TaxTypeCode);
                                        quotationDetail.TaxType = new TaxType();
                                        quotationDetail.TaxType.Code = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : quotationDetail.TaxType.Code);
                                        quotationDetail.TaxType.ValueText= (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : quotationDetail.TaxType.ValueText);
                                        quotationDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : quotationDetail.CGSTPerc);
                                        quotationDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : quotationDetail.SGSTPerc);
                                        quotationDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : quotationDetail.IGSTPerc);
                                    }
                                    quotationDetailList.Add(quotationDetail);
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

            return quotationDetailList;
        }


        #endregion GetQuotationDetailsByQuotationID
        #region Insert Update Quotation
        public object InsertUpdateQuotation(Quotation quotation)
        {
            SqlParameter outputStatus, outputID, outputQuotationNo = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateQuotation]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = quotation.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = quotation.ID;
                        cmd.Parameters.Add("@QuoteRefNo", SqlDbType.VarChar, 20).Value = quotation.QuoteRefNo;
                        cmd.Parameters.Add("@QuoteNo", SqlDbType.VarChar, 20).Value = quotation.QuoteNo;
                        cmd.Parameters.Add("@QuoteDate", SqlDbType.DateTime).Value = quotation.QuoteDateFormatted;
                        cmd.Parameters.Add("@EstimateID", SqlDbType.UniqueIdentifier).Value = quotation.EstimateID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = quotation.CustomerID;
                        cmd.Parameters.Add("@MailingAddress", SqlDbType.NVarChar, -1).Value = quotation.MailingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = quotation.ShippingAddress;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = quotation.DocumentStatusCode;
                        cmd.Parameters.Add("@ValidUpToDate", SqlDbType.DateTime).Value = quotation.ValidUpToDateFormatted;
                        cmd.Parameters.Add("@ReferredByCode", SqlDbType.Int).Value = quotation.ReferredByCode;
                        cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = quotation.PreparedBy;
                        cmd.Parameters.Add("@MailBodyHeader", SqlDbType.NVarChar, -1).Value = quotation.MailBodyHeader;
                        cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = quotation.MailBodyFooter;
                        //cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = quotation.EmailSentYN;
                        cmd.Parameters.Add("@LatestApprovalID", SqlDbType.UniqueIdentifier).Value = quotation.LatestApprovalID;
                        cmd.Parameters.Add("@IsFinalApproved", SqlDbType.Bit).Value = quotation.IsFinalApproved;
                        //cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar,-1).Value = quotation.EmailSentTo;
                        cmd.Parameters.Add("@TermReferenceNo", SqlDbType.VarChar,25).Value = quotation.TermReferenceNo;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = quotation.DetailXML;
                        cmd.Parameters.Add("@OtherChargeDetailXML", SqlDbType.Xml).Value = quotation.OtherChargeDetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = quotation.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = quotation.GeneralNotes;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = quotation.BranchCode;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = quotation.Discount;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = quotation.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = quotation.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = quotation.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = quotation.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputQuotationNo = cmd.Parameters.Add("@QuoteNoOut", SqlDbType.VarChar, 20);
                        outputQuotationNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        quotation.ID = Guid.Parse(outputID.Value.ToString());
                        quotation.QuoteNo = outputQuotationNo.Value.ToString();
                        return new
                        {
                            ID = quotation.ID,
                            QuotationNo = quotation.QuoteNo,
                            EstimateID=quotation.EstimateID,
                            Status = outputStatus.Value.ToString(),
                            Message = quotation.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = quotation.ID,
                QuotationNo = quotation.QuoteNo,
                EstimateID = quotation.EstimateID,
                Status = outputStatus.Value.ToString(),
                Message = quotation.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update Quotation
        #region Update Quotation Email Info
        public object UpdateQuotationEmailInfo(Quotation quotation)
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
                        cmd.CommandText = "[PSA].[UpdateQuotationEmailInfo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = quotation.ID;
                        cmd.Parameters.Add("@MailBodyHeader", SqlDbType.NVarChar, -1).Value = quotation.MailBodyHeader;
                        cmd.Parameters.Add("@MailBodyFooter", SqlDbType.NVarChar, -1).Value = quotation.MailBodyFooter;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = quotation.EmailSentYN;
                        cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar, -1).Value = quotation.EmailSentTo;
                        cmd.Parameters.Add("@Cc", SqlDbType.NVarChar, -1).Value = quotation.Cc;
                        cmd.Parameters.Add("@Bcc", SqlDbType.NVarChar, -1).Value = quotation.Bcc;
                        cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, -1).Value = quotation.Subject;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = quotation.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = quotation.PSASysCommon.UpdatedDate;
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
                            Message = quotation.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                Message = quotation.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Update Quotation Email Info
        #region Delete Quotation
        public object DeleteQuotation(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteQuotation]";
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
        #endregion Delete Quotation
        #region Delete Quotation Detail
        public object DeleteQuotationDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteQuotationDetail]";
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
        #endregion Delete Quotation Detail

        #region GetQuotationForSelectList
        public List<Quotation> GetQuotationForSelectList(Guid? quoteID)
        {
            List<Quotation> quotationList = null;
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
                        cmd.CommandText = "[PSA].[GetSelectListForQuotation]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if(quoteID==null)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = quoteID;
                        }
                        
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                quotationList = new List<Quotation>();
                                while (sdr.Read())
                                {
                                    Quotation quotation = new Quotation();
                                    {
                                        quotation.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotation.ID);
                                        quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : quotation.QuoteNo);
                                    }
                                    quotationList.Add(quotation);
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
            return quotationList;
        }
        #endregion GetQuotationForSelectList
        #region GetQuotationForSelectListOnDemand
        public List<Quotation> GetQuotationForSelectListOnDemand(string searchTerm)
        {
            List<Quotation> quotationList = null;
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
                        cmd.CommandText = "[PSA].[GetQuotationForSelectListOnDemand]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if(string.IsNullOrEmpty(searchTerm))
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
                                quotationList = new List<Quotation>();
                                while (sdr.Read())
                                {
                                    Quotation quotation = new Quotation();
                                    {
                                        quotation.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotation.ID);
                                        quotation.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? sdr["QuoteNo"].ToString() : quotation.QuoteNo);
                                    }
                                    quotationList.Add(quotation);
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
            return quotationList;
        }
        #endregion GetQuotationForSelectListOnDemand

        #region GetQuotationOtherChargeListByQuotationID
        public List<QuotationOtherCharge> GetQuotationOtherChargesDetailListByQuotationID(Guid quotationID)
        {
            List<QuotationOtherCharge> quotationOtherChargeList = new List<QuotationOtherCharge>();
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
                        cmd.CommandText = "[PSA].[GetQuotationOtherChargeListByQuotationID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@QuoteID", SqlDbType.UniqueIdentifier).Value = quotationID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    QuotationOtherCharge quotationOtherCharge = new QuotationOtherCharge();
                                    {
                                        quotationOtherCharge.ID= (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : quotationOtherCharge.ID);
                                        quotationOtherCharge.QuoteID = (sdr["QuoteID"].ToString() != "" ? Guid.Parse(sdr["QuoteID"].ToString()) : quotationOtherCharge.QuoteID);
                                        quotationOtherCharge.OtherChargeCode= (sdr["OtherChargeCode"].ToString() != "" ? int.Parse(sdr["OtherChargeCode"].ToString()) : quotationOtherCharge.OtherChargeCode);
                                        quotationOtherCharge.ChargeAmount = (sdr["ChargeAmount"].ToString() != "" ? decimal.Parse(sdr["ChargeAmount"].ToString()) : quotationOtherCharge.ChargeAmount);
                                        quotationOtherCharge.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : quotationOtherCharge.TaxTypeCode);
                                        quotationOtherCharge.TaxType = new TaxType();
                                        quotationOtherCharge.TaxType.Code = (sdr["TaxTypeCode"].ToString() != "" ? int.Parse(sdr["TaxTypeCode"].ToString()) : quotationOtherCharge.TaxType.Code);
                                        quotationOtherCharge.TaxType.ValueText = (sdr["TaxTypeText"].ToString() != "" ? (sdr["TaxTypeText"].ToString()) : quotationOtherCharge.TaxType.ValueText);
                                        quotationOtherCharge.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : quotationOtherCharge.CGSTPerc);
                                        quotationOtherCharge.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : quotationOtherCharge.SGSTPerc);
                                        quotationOtherCharge.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : quotationOtherCharge.IGSTPerc);
                                        quotationOtherCharge.OtherCharge = new OtherCharge();
                                        quotationOtherCharge.OtherCharge.Description= (sdr["OtherCharge"].ToString() != "" ? sdr["OtherCharge"].ToString() : quotationOtherCharge.OtherCharge.Description);
                                        quotationOtherCharge.OtherCharge.SACCode = (sdr["SACCode"].ToString() != "" ? sdr["SACCode"].ToString() : quotationOtherCharge.OtherCharge.SACCode);
                                    }
                                    quotationOtherChargeList.Add(quotationOtherCharge);
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
            return quotationOtherChargeList;
        }
        #endregion GetQuotationOtherChargeListByQuotationID

        #region Delete Quotation OtherCharge
        public object DeleteQuotationOtherChargeDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteQuotationOtherCharge]";
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
        #endregion Delete Quotation OtherCharge

        #region GetQuotationSummaryCount
        public QuotationSummary GetQuotationSummaryCount()
        {
            QuotationSummary quotationSummary = null;
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
                        cmd.CommandText = "[PSA].[GetQuotationSummaryCount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    quotationSummary = new QuotationSummary();
                                    quotationSummary.TotalQuotationCount = (sdr["TotalQuotation"].ToString() != "" ? int.Parse(sdr["TotalQuotation"].ToString()) : quotationSummary.TotalQuotationCount);
                                    quotationSummary.LostQuotationCount = (sdr["TotalLostQuotation"].ToString() != "" ? int.Parse(sdr["TotalLostQuotation"].ToString()) : quotationSummary.LostQuotationCount);
                                    quotationSummary.ConvertedQuotationCount = (sdr["TotalConvertedQuotation"].ToString() != "" ? int.Parse(sdr["TotalConvertedQuotation"].ToString()) : quotationSummary.ConvertedQuotationCount);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return quotationSummary;
        }
        #endregion GetQuotationSummaryCount
    }
}

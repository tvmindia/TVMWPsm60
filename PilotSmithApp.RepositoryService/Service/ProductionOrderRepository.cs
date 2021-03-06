﻿using PilotSmithApp.DataAccessObject.DTO;
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
    public class ProductionOrderRepository: IProductionOrderRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ProductionOrderRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get All ProductionOrder
        public List<ProductionOrder> GetAllProductionOrder(ProductionOrderAdvanceSearch productionOrderAdvanceSearch)
        {
            List<ProductionOrder> productionOrderList = null;
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
                        cmd.CommandText = "[PSA].[GetAllProductionOrder]";
                        if (string.IsNullOrEmpty(productionOrderAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = productionOrderAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionOrderAdvanceSearch.DataTablePaging.Start;
                        if (productionOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = productionOrderAdvanceSearch.AdvFromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = productionOrderAdvanceSearch.AdvToDate;
                        if (productionOrderAdvanceSearch.AdvCustomerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = productionOrderAdvanceSearch.AdvCustomerID;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = productionOrderAdvanceSearch.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionOrderAdvanceSearch.AdvBranchCode;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionOrderAdvanceSearch.AdvDocumentStatusCode;
                        if (productionOrderAdvanceSearch.AdvDocumentOwnerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@DocumentOwnerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionOrderAdvanceSearch.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@ApprovalStatusCode", SqlDbType.Int).Value = productionOrderAdvanceSearch.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.NVarChar).Value = productionOrderAdvanceSearch.AdvEmailSentStatus;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionOrderList = new List<ProductionOrder>();
                                while (sdr.Read())
                                {
                                    ProductionOrder productionOrder = new ProductionOrder();
                                    {
                                        productionOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionOrder.ID);
                                        productionOrder.SaleOrderID= (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : productionOrder.SaleOrderID);
                                        productionOrder.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionOrder.ProdOrderNo);
                                        productionOrder.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : productionOrder.ProdOrderDate);
                                        productionOrder.ProdOrderDateFormatted = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()).ToString(_settings.DateFormat) : productionOrder.ProdOrderDateFormatted);
                                        productionOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionOrder.CustomerID);
                                        productionOrder.Customer = new Customer();
                                        productionOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionOrder.Customer.ID);
                                        productionOrder.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : productionOrder.Customer.CompanyName);
                                        productionOrder.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : productionOrder.Customer.ContactPerson);
                                        productionOrder.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : productionOrder.Customer.Mobile);
                                        productionOrder.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionOrder.DocumentStatusCode);
                                        productionOrder.DocumentStatus = new DocumentStatus();
                                        productionOrder.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionOrder.DocumentStatus.Code);
                                        productionOrder.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : productionOrder.DocumentStatus.Description);
                                       // productionOrder.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionOrder.GeneralNotes);
                                        productionOrder.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : productionOrder.DocumentOwnerID);
                                        productionOrder.Branch = new Branch();
                                        productionOrder.Branch.Description = (sdr["BranchDescription"].ToString() != "" ? sdr["BranchDescription"].ToString() : productionOrder.Branch.Description);
                                        productionOrder.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : productionOrder.BranchCode);
                                        productionOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productionOrder.FilteredCount);
                                        productionOrder.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionOrder.FilteredCount);
                                        productionOrder.Area = new Area();
                                        productionOrder.Area.Description= (sdr["Area"].ToString() != "" ? sdr["Area"].ToString() : productionOrder.Area.Description);
                                        productionOrder.ApprovalStatus = new ApprovalStatus();
                                        productionOrder.ApprovalStatus.Description= (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : productionOrder.ApprovalStatus.Description);
                                        productionOrder.PSAUser = new PSAUser();
                                        productionOrder.PSAUser.LoginName = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : productionOrder.PSAUser.LoginName);
                                        productionOrder.EmailSentYN= (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : productionOrder.EmailSentYN);
                                        productionOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : productionOrder.SaleOrderNo);
                                    }
                                    productionOrderList.Add(productionOrder);
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

            return productionOrderList;
        }
        #endregion Get All ProductionOrder
        #region Get ProductionOrder
        public ProductionOrder GetProductionOrder(Guid id)
        {
            ProductionOrder productionOrder = new ProductionOrder();
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
                        cmd.CommandText = "[PSA].[GetProductionOrder]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    productionOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionOrder.ID);
                                    productionOrder.SaleOrderID= (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : productionOrder.SaleOrderID);
                                    productionOrder.SaleOrderRefNo = (sdr["SaleOrderRefNo"].ToString() != "" ? sdr["SaleOrderRefNo"].ToString() : productionOrder.SaleOrderRefNo);
                                    productionOrder.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionOrder.ProdOrderNo);
                                    productionOrder.ProdOrderRefNo = (sdr["ProdOrderRefNo"].ToString() != "" ? sdr["ProdOrderRefNo"].ToString() : productionOrder.ProdOrderRefNo);
                                    productionOrder.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : productionOrder.ProdOrderDate);
                                    productionOrder.ProdOrderDateFormatted = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()).ToString(_settings.DateFormat) : productionOrder.ProdOrderDateFormatted);
                                    productionOrder.ExpectedDelvDate= (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : productionOrder.ExpectedDelvDate);
                                    productionOrder.ExpectedDelvDateFormatted= (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : productionOrder.ExpectedDelvDateFormatted);
                                    productionOrder.PreparedBy = (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : productionOrder.PreparedBy);
                                    productionOrder.Customer = new Customer();
                                    productionOrder.Customer.CompanyName = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : productionOrder.Customer.CompanyName);
                                    productionOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionOrder.CustomerID);
                                    productionOrder.Customer.ID= (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionOrder.Customer.ID);
                                    productionOrder.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionOrder.DocumentStatusCode);
                                    productionOrder.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionOrder.GeneralNotes);
                                    productionOrder.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : productionOrder.DocumentOwnerID);
                                    productionOrder.Branch = new Branch();
                                    productionOrder.Branch.Description = (sdr["Branch"].ToString() != "" ? sdr["Branch"].ToString() : productionOrder.Branch.Description);
                                    productionOrder.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : productionOrder.BranchCode);
                                    productionOrder.Branch.Code= (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : productionOrder.Branch.Code);
                                    productionOrder.DocumentStatus = new DocumentStatus();
                                    productionOrder.DocumentStatus.Description = (sdr["DocumentStatus"].ToString() != "" ? sdr["DocumentStatus"].ToString() : productionOrder.DocumentStatus.Description);
                                    productionOrder.DocumentOwners = (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : productionOrder.DocumentOwners);
                                    productionOrder.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? sdr["DocumentOwner"].ToString() : productionOrder.DocumentOwner);
                                    productionOrder.EmailSentTo = (sdr["EmailSentTo"].ToString() != "" ? sdr["EmailSentTo"].ToString() : productionOrder.EmailSentTo);
                                    productionOrder.Cc = (sdr["Cc"].ToString() != "" ? (sdr["Cc"].ToString()) : productionOrder.Cc);
                                    productionOrder.Bcc = (sdr["Bcc"].ToString() != "" ? (sdr["Bcc"].ToString()) : productionOrder.Bcc);
                                    productionOrder.Subject = (sdr["Subject"].ToString() != "" ? (sdr["Subject"].ToString()) : productionOrder.Subject);
                                    productionOrder.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : productionOrder.EmailSentYN);
                                    //productionOrder.MailBodyHeader = (sdr["MailBodyHeader"].ToString() != "" ? sdr["MailBodyHeader"].ToString() : productionOrder.MailBodyHeader);
                                    //productionOrder.MailBodyFooter = (sdr["MailBodyFooter"].ToString() != "" ? sdr["MailBodyFooter"].ToString() : productionOrder.MailBodyFooter);
                                    productionOrder.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : productionOrder.LatestApprovalID);
                                    productionOrder.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : productionOrder.LatestApprovalStatus);
                                    productionOrder.LatestApprovalStatusDescription = (sdr["ApprovalDescription"].ToString() != "" ? (sdr["ApprovalDescription"].ToString()) : productionOrder.LatestApprovalStatusDescription);
                                    productionOrder.IsFinalApproved = (sdr["IsFinalApproved"].ToString() != "" ? bool.Parse(sdr["IsFinalApproved"].ToString()) : productionOrder.IsFinalApproved);
                                    string mailFrom = (sdr["MailFromAddress"].ToString() != "" ? sdr["MailFromAddress"].ToString() : productionOrder.MailFrom);
                                    productionOrder.MailFrom = mailFrom.Replace("\n", "<br />");
                                    productionOrder.ApproverLevel = (sdr["ApproverLevel"].ToString() != "" ? int.Parse(sdr["ApproverLevel"].ToString()) : productionOrder.ApproverLevel);
                                    productionOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : productionOrder.SaleOrderNo);
                                    productionOrder.CurrencyCode = (sdr["CurrencyCode"].ToString() != "" ? sdr["CurrencyCode"].ToString() : productionOrder.CurrencyCode);
                                    productionOrder.CurrencyRate = (sdr["CurrencyRate"].ToString() != "" ? Decimal.Parse(sdr["CurrencyRate"].ToString()) : productionOrder.CurrencyRate);
                                    productionOrder.Currency = new Currency();
                                    productionOrder.Currency.Description = (sdr["CurrencyDescription"].ToString() != "" ? sdr["CurrencyDescription"].ToString() : productionOrder.Currency.Description);
                                    productionOrder.PreparedByName = (sdr["PreparedByName"].ToString() != "" ? (sdr["PreparedByName"].ToString()) : productionOrder.PreparedByName);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return productionOrder;
        }
        #endregion Get ProductionOrder
        #region GetAllProductionOrder Details
        public List<ProductionOrderDetail> GetProductionOrderDetailListByProductionOrderID(Guid productionOrderID)
        {
            List<ProductionOrderDetail> productionOrderDetailList = new List<ProductionOrderDetail>();
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
                        cmd.CommandText = "[PSA].[GetProductionOrderDetailListByProductionOrderID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ProdOrderID", SqlDbType.UniqueIdentifier).Value = productionOrderID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    ProductionOrderDetail productionOrderDetail = new ProductionOrderDetail();
                                    {
                                        productionOrderDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionOrderDetail.ID);
                                        productionOrderDetail.ProdOrderID = (sdr["ProdOrderID"].ToString() != "" ? Guid.Parse(sdr["ProdOrderID"].ToString()) : productionOrderDetail.ProdOrderID);
                                        productionOrderDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : productionOrderDetail.ProductSpec);
                                        productionOrderDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty),
                                            HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : String.Empty)
                                        };
                                        productionOrderDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        productionOrderDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        productionOrderDetail.ProductModel = new ProductModel();
                                        productionOrderDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        productionOrderDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : productionOrderDetail.ProductModel.Name);
                                        productionOrderDetail.OrderQty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : productionOrderDetail.OrderQty);
                                        productionOrderDetail.ProducedQty = (sdr["ProducedQty"].ToString() != "" ? decimal.Parse(sdr["ProducedQty"].ToString()) : productionOrderDetail.ProducedQty);
                                        productionOrderDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : productionOrderDetail.Rate);
                                        productionOrderDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : productionOrderDetail.UnitCode);
                                        productionOrderDetail.Unit = new Unit();
                                        productionOrderDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : productionOrderDetail.Unit.Code);
                                        productionOrderDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : productionOrderDetail.Unit.Description);
                                        productionOrderDetail.PlantCode= (sdr["PlantCode"].ToString() != "" ? int.Parse(sdr["PlantCode"].ToString()) : productionOrderDetail.PlantCode);
                                        productionOrderDetail.Plant = new Plant();
                                        productionOrderDetail.Plant.Code = (sdr["PlantCode"].ToString() != "" ? int.Parse(sdr["PlantCode"].ToString()) : productionOrderDetail.Plant.Code);
                                        productionOrderDetail.Plant.Description = (sdr["PlantDescription"].ToString() != "" ? (sdr["PlantDescription"].ToString()) : productionOrderDetail.Plant.Description);
                                        productionOrderDetail.QCCompletedQty= (sdr["QCCompletedQty"].ToString() != "" ? decimal.Parse(sdr["QCCompletedQty"].ToString()) : productionOrderDetail.QCCompletedQty);
                                        productionOrderDetail.MileStone1FcFinishDt= (sdr["MileStone1FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone1FcFinishDt"].ToString()) : productionOrderDetail.MileStone1FcFinishDt);
                                        productionOrderDetail.MileStone1FcFinishDtFormatted= (sdr["MileStone1FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone1FcFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone1FcFinishDtFormatted==null?"": productionOrderDetail.MileStone1FcFinishDtFormatted);
                                        productionOrderDetail.MileStone1AcTFinishDt = (sdr["MileStone1AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone1AcTFinishDt"].ToString()) : productionOrderDetail.MileStone1AcTFinishDt);
                                        productionOrderDetail.MileStone1AcTFinishDtFormatted = (sdr["MileStone1AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone1AcTFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone1AcTFinishDtFormatted==null?"": productionOrderDetail.MileStone1AcTFinishDtFormatted);
                                        productionOrderDetail.MileStone2FcFinishDt = (sdr["MileStone2FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone2FcFinishDt"].ToString()) : productionOrderDetail.MileStone2FcFinishDt);
                                        productionOrderDetail.MileStone2FcFinishDtFormatted = (sdr["MileStone2FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone2FcFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone2FcFinishDtFormatted==null?"" : productionOrderDetail.MileStone2FcFinishDtFormatted);
                                        productionOrderDetail.MileStone2AcTFinishDt = (sdr["MileStone2AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone2AcTFinishDt"].ToString()) : productionOrderDetail.MileStone2AcTFinishDt);
                                        productionOrderDetail.MileStone2AcTFinishDtFormatted = (sdr["MileStone2AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone2AcTFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone2AcTFinishDtFormatted==null?"": productionOrderDetail.MileStone2AcTFinishDtFormatted);
                                        productionOrderDetail.MileStone3FcFinishDt = (sdr["MileStone3FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone3FcFinishDt"].ToString()) : productionOrderDetail.MileStone3FcFinishDt);
                                        productionOrderDetail.MileStone3FcFinishDtFormatted = (sdr["MileStone3FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone3FcFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone3FcFinishDtFormatted==null?"": productionOrderDetail.MileStone3FcFinishDtFormatted);
                                        productionOrderDetail.MileStone3AcTFinishDt = (sdr["MileStone3AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone3AcTFinishDt"].ToString()) : productionOrderDetail.MileStone3AcTFinishDt);
                                        productionOrderDetail.MileStone3AcTFinishDtFormatted = (sdr["MileStone3AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone3AcTFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone3AcTFinishDtFormatted==null?"": productionOrderDetail.MileStone3AcTFinishDtFormatted);
                                        productionOrderDetail.MileStone4FcFinishDt = (sdr["MileStone4FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone4FcFinishDt"].ToString()) : productionOrderDetail.MileStone4FcFinishDt);
                                        productionOrderDetail.MileStone4FcFinishDtFormatted = (sdr["MileStone4FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone4FcFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone4FcFinishDtFormatted==null?"": productionOrderDetail.MileStone4FcFinishDtFormatted);
                                        productionOrderDetail.MileStone4AcTFinishDt = (sdr["MileStone4AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone4AcTFinishDt"].ToString()) : productionOrderDetail.MileStone4AcTFinishDt);
                                        productionOrderDetail.MileStone4AcTFinishDtFormatted = (sdr["MileStone4AcTFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone4AcTFinishDt"].ToString()).ToString(_settings.DateFormat) : productionOrderDetail.MileStone4AcTFinishDtFormatted==null?"": productionOrderDetail.MileStone4AcTFinishDtFormatted);
                                        productionOrderDetail.SpecTag= (sdr["SpecTag"].ToString() != "" ? Guid.Parse(sdr["SpecTag"].ToString()) : Guid.Empty);
                                        productionOrderDetail.PrevProdOrderQty = (sdr["PrevProdOrderQty"].ToString() != "" ? decimal.Parse(sdr["PrevProdOrderQty"].ToString()) : productionOrderDetail.PrevProdOrderQty);
                                        productionOrderDetail.PrevProducedQty= (sdr["PrevProducedQty"].ToString() != "" ? decimal.Parse(sdr["PrevProducedQty"].ToString()) : productionOrderDetail.PrevProducedQty);
                                        productionOrderDetail.PrevDelQty = (sdr["PrevDelQty"].ToString() != "" ? decimal.Parse(sdr["PrevDelQty"].ToString()) : productionOrderDetail.PrevDelQty);
                                        //productionOrderDetail.SaleOrderDetail = new SaleOrderDetail();
                                        productionOrderDetail.SaleOrderQty= (sdr["SaleOrderQty"].ToString() != "" ? decimal.Parse(sdr["SaleOrderQty"].ToString()) : productionOrderDetail.SaleOrderDetail.Qty);
                                        //productionOrderDetail.TotalProdusedQty = (sdr["TotalProducedQty"].ToString() != "" ? decimal.Parse(sdr["TotalProducedQty"].ToString()) : productionOrderDetail.TotalProdusedQty);
                                        productionOrderDetail.SaleOrderDetailID= (sdr["SaleOrderDetailID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderDetailID"].ToString()) : productionOrderDetail.SaleOrderDetailID);
                                    }
                                    productionOrderDetailList.Add(productionOrderDetail);
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

            return productionOrderDetailList;
        }
        #endregion GetAllProductionOrder Details
        #region Insert Update ProductionOrder
        public object InsertUpdateProductionOrder(ProductionOrder productionOrder)
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
                        cmd.CommandText = "[PSA].[InsertUpdateProductionOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = productionOrder.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = productionOrder.ID;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = productionOrder.SaleOrderID;
                        cmd.Parameters.Add("@ProdOrderNo", SqlDbType.VarChar, 20).Value = productionOrder.ProdOrderNo;
                        cmd.Parameters.Add("@ProdOrderRefNo", SqlDbType.VarChar, 20).Value = productionOrder.ProdOrderRefNo;
                        cmd.Parameters.Add("@ProdOrderDate", SqlDbType.DateTime).Value = productionOrder.ProdOrderDateFormatted;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = productionOrder.CustomerID;
                        cmd.Parameters.Add("@ExpectedDelvDate", SqlDbType.DateTime).Value = productionOrder.ExpectedDelvDateFormatted;
                        cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = productionOrder.PreparedBy;
                       // cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionOrder.DocumentStatusCode;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = productionOrder.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = productionOrder.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = productionOrder.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionOrder.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionOrder.BranchCode;
                        cmd.Parameters.Add("@LatestApprovalID", SqlDbType.UniqueIdentifier).Value = productionOrder.LatestApprovalID;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = productionOrder.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = productionOrder.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = productionOrder.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = productionOrder.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@CurrencyCode", SqlDbType.VarChar).Value = productionOrder.CurrencyCode;
                        cmd.Parameters.Add("@CurrencyRate", SqlDbType.Decimal).Value = productionOrder.CurrencyRate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputProdOrderNo = cmd.Parameters.Add("@ProdOrderNoOut", SqlDbType.VarChar, 20);
                        outputProdOrderNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        productionOrder.ID = Guid.Parse(outputID.Value.ToString());
                        productionOrder.ProdOrderNo = outputProdOrderNo.Value.ToString();
                        return new
                        {
                            ID = productionOrder.ID,
                            ProductionOrderNo = productionOrder.ProdOrderNo,
                            SaleOrderID=productionOrder.SaleOrderID,
                            Status = outputStatus.Value.ToString(),
                            Message = productionOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = productionOrder.ID,
                ProductionOrderNo = productionOrder.ProdOrderNo,
                Status = outputStatus.Value.ToString(),
                Message = productionOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update ProductionOrder
        #region Delete ProductionOrder
        public object DeleteProductionOrder(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteProductionOrder]";
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
        #endregion Delete ProductionOrder
        #region Delete ProductionOrder Detail
        public object DeleteProductionOrderDetail(Guid id, string CreatedBy, DateTime CreatedDate)
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
                        cmd.CommandText = "[PSA].[DeleteProductionOrderDetail]";
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
        #endregion Delete ProductionOrder Detail
        #region GetProductionOrderForSelectListOnDemand
        public List<ProductionOrder> GetProductionOrderForSelectListOnDemand(string searchTerm)
        {
            List<ProductionOrder> productionOrderList = null;
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
                        cmd.CommandText = "[PSA].[GetProductionOrderForSelectListOnDemand]";
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
                                productionOrderList = new List<ProductionOrder>();
                                while (sdr.Read())
                                {
                                    ProductionOrder productionOrder = new ProductionOrder();
                                    {
                                        productionOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionOrder.ID);
                                        productionOrder.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionOrder.ProdOrderNo);
                                    }
                                    productionOrderList.Add(productionOrder);
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
            return productionOrderList;
        }
        #endregion GetProductionOrderForSelectListOnDemand
        #region GetProductionOrderForSelectList
        public List<ProductionOrder> GetProductionOrderForSelectList(Guid? id)
        {
            List<ProductionOrder> productionOrderList = null;
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
                        cmd.CommandText = "[PSA].[GetProductionOrderForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (id==null)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        }
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionOrderList = new List<ProductionOrder>();
                                while (sdr.Read())
                                {
                                    ProductionOrder productionOrder = new ProductionOrder();
                                    {
                                        productionOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionOrder.ID);
                                        productionOrder.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionOrder.ProdOrderNo);
                                    }
                                    productionOrderList.Add(productionOrder);
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
            return productionOrderList;
        }
        #endregion GetProductionOrderForSelectList
        #region UpdateProductionOrderEmailInfo
        public object UpdateProductionOrderEmailInfo(ProductionOrder productionOrder,int emailSendSuccess=0)
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
                        cmd.CommandText = "[PSA].[UpdateProductionOrderEmailInfo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = productionOrder.ID;                      
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.Bit).Value = productionOrder.EmailSentYN;
                        cmd.Parameters.Add("@EmailSentTo", SqlDbType.NVarChar, -1).Value = productionOrder.EmailSentTo;
                        cmd.Parameters.Add("@Cc", SqlDbType.NVarChar, -1).Value = productionOrder.Cc;
                        cmd.Parameters.Add("@Bcc", SqlDbType.NVarChar, -1).Value = productionOrder.Bcc;
                        cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, -1).Value = productionOrder.Subject;
                        cmd.Parameters.Add("@emilSendSuccess", SqlDbType.Int).Value = emailSendSuccess;
                        cmd.Parameters.Add("@itemlist", SqlDbType.NVarChar, -1).Value = productionOrder.EmailItemList;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = productionOrder.LatestApprovalStatus;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = productionOrder.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = productionOrder.PSASysCommon.UpdatedDate;
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
                            Message = productionOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                Message = productionOrder.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion UpdateProductionOrderEmailInfo

        #region GetProductionOrderSummaryCount
        public ProductionOrderSummary GetProductionOrderSummaryCount()
        {
            ProductionOrderSummary productionOrderSummary = null;
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
                        cmd.CommandText = "[PSA].[GetProductionOrderSummaryCount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    productionOrderSummary = new ProductionOrderSummary();
                                    productionOrderSummary.TotalProductionOrderCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionOrderSummary.TotalProductionOrderCount);
                                    productionOrderSummary.OpenProductionOrderCount = (sdr["OpenCount"].ToString() != "" ? int.Parse(sdr["OpenCount"].ToString()) : productionOrderSummary.OpenProductionOrderCount);
                                    productionOrderSummary.ClosedProductionOrderCount = (sdr["ClosedCount"].ToString() != "" ? int.Parse(sdr["ClosedCount"].ToString()) : productionOrderSummary.ClosedProductionOrderCount);
                                    productionOrderSummary.InProgressProductionOrderCount = (sdr["InProgressCount"].ToString() != "" ? int.Parse(sdr["InProgressCount"].ToString()) : productionOrderSummary.InProgressProductionOrderCount);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionOrderSummary;
        }
        #endregion GetProductionOrderSummaryCount

        #region ValidateProductionOrderDetailOrderQty
        public ProductionOrderDetail ValidateProductionOrderDetailOrderQty(Guid SaleOrderDetailID, Guid ProductionOrderDetailID)
        {
            ProductionOrderDetail productionOrderDetail = new ProductionOrderDetail();
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
                        cmd.CommandText = "[PSA].[ValidateProdutionOrderOrderQtyDetail]";
                        cmd.Parameters.Add("@SaleOrderDetailID", SqlDbType.UniqueIdentifier).Value = SaleOrderDetailID;
                        if(ProductionOrderDetailID != Guid.Empty)
                        cmd.Parameters.Add("@ProductionOrderDetailID", SqlDbType.UniqueIdentifier).Value = ProductionOrderDetailID;
                        else
                            cmd.Parameters.AddWithValue("@ProductionOrderDetailID", DBNull.Value);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    //productionOrderDetail.PrevProducedQty= (sdr["PrevProducedQty"].ToString() != "" ? decimal.Parse(sdr["PrevProducedQty"].ToString()) : productionOrderDetail.PrevProducedQty);
                                    productionOrderDetail.TotalProdOrderQty = (sdr["TotalProdOrderQty"].ToString() != "" ? decimal.Parse(sdr["TotalProdOrderQty"].ToString()) : productionOrderDetail.TotalProdOrderQty);
                                    productionOrderDetail.SaleOrderQty= (sdr["SaleOrderQty"].ToString() != "" ? decimal.Parse(sdr["SaleOrderQty"].ToString()) : productionOrderDetail.SaleOrderDetail.Qty);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return productionOrderDetail;
        }
        #endregion ValidateProductionOrderDetailOrderQty
    }
}

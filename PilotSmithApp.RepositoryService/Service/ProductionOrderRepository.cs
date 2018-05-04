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
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = productionOrderAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionOrderAdvanceSearch.DataTablePaging.Start;
                        if (productionOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = productionOrderAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = productionOrderAdvanceSearch.ToDate;
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
                                        productionOrder.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionOrder.ProdOrderNo);
                                        productionOrder.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : productionOrder.ProdOrderDate);
                                        productionOrder.ProdOrderDateFormatted = (sdr["ProdOrderDateFormatted"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDateFormatted"].ToString()).ToString(_settings.DateFormat) : productionOrder.ProdOrderDateFormatted);
                                        productionOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionOrder.CustomerID);
                                        productionOrder.Customer = new Customer();
                                        productionOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionOrder.Customer.ID);
                                        productionOrder.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : productionOrder.Customer.CompanyName);
                                        productionOrder.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : productionOrder.Customer.ContactPerson);
                                        productionOrder.Customer.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : productionOrder.Customer.Mobile);
                                        productionOrder.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionOrder.DocumentStatusCode);
                                        productionOrder.DocumentStatus = new DocumentStatus();
                                        productionOrder.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionOrder.DocumentStatus.Code);
                                        productionOrder.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : productionOrder.DocumentStatus.Description);
                                        productionOrder.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionOrder.GeneralNotes);
                                        productionOrder.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : productionOrder.DocumentOwnerID);
                                        productionOrder.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : productionOrder.BranchCode);
                                        productionOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productionOrder.FilteredCount);
                                        productionOrder.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionOrder.FilteredCount);
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
                                    productionOrder.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionOrder.ProdOrderNo);
                                    productionOrder.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : productionOrder.ProdOrderDate);
                                    productionOrder.ProdOrderDateFormatted = (sdr["ProdOrderDateFormatted"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDateFormatted"].ToString()).ToString("dd-MMM-yyyy") : productionOrder.ProdOrderDateFormatted);
                                    productionOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionOrder.CustomerID);
                                    productionOrder.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionOrder.DocumentStatusCode);
                                    productionOrder.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionOrder.GeneralNotes);
                                    productionOrder.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : productionOrder.DocumentOwnerID);
                                    productionOrder.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : productionOrder.BranchCode);

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
        #region GetAllProductionOrderItems
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
                        cmd.Parameters.Add("@ProductionOrderID", SqlDbType.UniqueIdentifier).Value = productionOrderID;
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
                                        productionOrderDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : productionOrderDetail.ProductSpec);
                                        productionOrderDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty)
                                        };
                                        productionOrderDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        productionOrderDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        productionOrderDetail.ProductModel = new ProductModel();
                                        productionOrderDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        productionOrderDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : productionOrderDetail.ProductModel.Name);
                                        productionOrderDetail.OrderQty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : productionOrderDetail.OrderQty);
                                        productionOrderDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : productionOrderDetail.Rate);
                                        productionOrderDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : productionOrderDetail.UnitCode);
                                        productionOrderDetail.Unit = new Unit();
                                        productionOrderDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : productionOrderDetail.Unit.Code);
                                        productionOrderDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : productionOrderDetail.Unit.Description);
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


        #endregion GetQuotationDetails
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
                        cmd.Parameters.Add("@ProdOrderNo", SqlDbType.VarChar, 20).Value = productionOrder.ProdOrderNo;
                        cmd.Parameters.Add("@ProdOrderDate", SqlDbType.DateTime).Value = productionOrder.ProdOrderDateFormatted;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = productionOrder.CustomerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionOrder.DocumentStatusCode;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = productionOrder.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = productionOrder.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = productionOrder.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionOrder.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionOrder.BranchCode;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = productionOrder.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = productionOrder.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = productionOrder.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = productionOrder.PSASysCommon.CreatedDate;
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
        public object DeleteProductionOrderDetail(Guid id)
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
    }
}

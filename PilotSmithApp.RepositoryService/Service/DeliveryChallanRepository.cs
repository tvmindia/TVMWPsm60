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
    public class DeliveryChallanRepository:IDeliveryChallanRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        public DeliveryChallanRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region Get All DeliveryChallan
        public List<DeliveryChallan> GetAllDeliveryChallan(DeliveryChallanAdvanceSearch deliveryChallanAdvanceSearch)
        {
            List<DeliveryChallan> deliveryChallanList = null;
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
                        cmd.CommandText = "[PSA].[GetAllDeliveryChallan]";
                        if (string.IsNullOrEmpty(deliveryChallanAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = deliveryChallanAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = deliveryChallanAdvanceSearch.DataTablePaging.Start;
                        if (deliveryChallanAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = deliveryChallanAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = deliveryChallanAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = deliveryChallanAdvanceSearch.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                deliveryChallanList = new List<DeliveryChallan>();
                                while (sdr.Read())
                                {
                                    DeliveryChallan deliveryChallan = new DeliveryChallan();
                                    {
                                        deliveryChallan.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : deliveryChallan.ID);
                                        deliveryChallan.DelvChallanNo = (sdr["DelvChallanNo"].ToString() != "" ? sdr["DelvChallanNo"].ToString() : deliveryChallan.DelvChallanNo);
                                        deliveryChallan.DelvChallanRefNo = (sdr["DelvChallanRefNo"].ToString() != "" ? sdr["DelvChallanRefNo"].ToString() : deliveryChallan.DelvChallanRefNo);
                                        deliveryChallan.DelvChallanDate = (sdr["DelvChallanDate"].ToString() != "" ? DateTime.Parse(sdr["DelvChallanDate"].ToString()) : deliveryChallan.DelvChallanDate);
                                        deliveryChallan.DelvChallanDateFormatted = (sdr["DelvChallanDate"].ToString() != "" ? DateTime.Parse(sdr["DelvChallanDate"].ToString()).ToString(_settings.DateFormat) : deliveryChallan.DelvChallanDateFormatted);
                                        deliveryChallan.SaleOrder = new SaleOrder();
                                        deliveryChallan.SaleOrder.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? sdr["SaleOrderNo"].ToString() : deliveryChallan.SaleOrder.SaleOrderNo);
                                        deliveryChallan.ProductionOrder = new ProductionOrder();
                                        deliveryChallan.ProductionOrder.ProdOrderNo= (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : deliveryChallan.ProductionOrder.ProdOrderNo);
                                        deliveryChallan.Customer = new Customer();
                                        deliveryChallan.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : deliveryChallan.Customer.CompanyName);
                                        deliveryChallan.Plant = new Plant();
                                        deliveryChallan.Plant.Description = (sdr["Plant"].ToString() != "" ? sdr["Plant"].ToString() : deliveryChallan.Plant.Description);
                                        deliveryChallan.Employee = new Employee();
                                        deliveryChallan.Employee.Name = (sdr["PreparedBy"].ToString() != "" ? sdr["PreparedBy"].ToString() : deliveryChallan.Employee.Name);
                                        deliveryChallan.Branch = new Branch();
                                        deliveryChallan.Branch.Description = (sdr["Branch"].ToString() != "" ? sdr["PreparedBy"].ToString() : deliveryChallan.Branch.Description);
                                        deliveryChallan.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : deliveryChallan.FilteredCount);
                                        deliveryChallan.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : deliveryChallan.FilteredCount);
                                    }
                                    deliveryChallanList.Add(deliveryChallan);
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

            return deliveryChallanList;
        }
        #endregion Get All DeliveryChallan

        #region Get DeliveryChallan
        public DeliveryChallan GetDeliveryChallan (Guid id)
        {
            DeliveryChallan deliveryChallan = new DeliveryChallan();
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
                        cmd.CommandText = "[PSA].[GetDeliveryChallan]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    deliveryChallan.ID= (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : deliveryChallan.ID);
                                    deliveryChallan.DelvChallanNo = (sdr["DelvChallanNo"].ToString() != "" ? sdr["DelvChallanNo"].ToString() : deliveryChallan.DelvChallanNo);
                                    deliveryChallan.DelvChallanRefNo = (sdr["DelvChallanRefNo"].ToString() != "" ? sdr["DelvChallanRefNo"].ToString() : deliveryChallan.DelvChallanRefNo);
                                    deliveryChallan.DelvChallanDate = (sdr["DelvChallanDate"].ToString() != "" ? DateTime.Parse(sdr["DelvChallanDate"].ToString()) : deliveryChallan.DelvChallanDate);
                                    deliveryChallan.DelvChallanDateFormatted = (sdr["DelvChallanDate"].ToString() != "" ? DateTime.Parse(sdr["DelvChallanDate"].ToString()).ToString(_settings.DateFormat) : deliveryChallan.DelvChallanDateFormatted);
                                    deliveryChallan.SaleOrderID= (sdr["SaleOrderID"].ToString() != "" ? Guid.Parse(sdr["SaleOrderID"].ToString()) : deliveryChallan.SaleOrderID);
                                    deliveryChallan.ProdOrderID= (sdr["ProdOrderID"].ToString() != "" ? Guid.Parse(sdr["ProdOrderID"].ToString()) : deliveryChallan.ProdOrderID);
                                    deliveryChallan.CustomerID= (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : deliveryChallan.CustomerID);
                                    deliveryChallan.PlantCode = (sdr["PlantCode"].ToString() != "" ? int.Parse(sdr["PlantCode"].ToString()) : deliveryChallan.PlantCode);
                                    deliveryChallan.PreparedBy= (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : deliveryChallan.PreparedBy);
                                    deliveryChallan.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? (sdr["GeneralNotes"]).ToString() : deliveryChallan.GeneralNotes);
                                    deliveryChallan.DocumentOwnerID = deliveryChallan.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : deliveryChallan.DocumentOwnerID);
                                    deliveryChallan.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : deliveryChallan.BranchCode);
                                    deliveryChallan.VehiclePlateNo = (sdr["VehiclePlateNo"].ToString() != "" ? sdr["VehiclePlateNo"].ToString() : deliveryChallan.VehiclePlateNo);
                                    deliveryChallan.DriverName = (sdr["DriverName"].ToString() != "" ? sdr["DriverName"].ToString() : deliveryChallan.DriverName);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return deliveryChallan;
        }
        #endregion Get DeliveryChallan

        #region GetDeliveryChallanDetailListByDeliveryChallanID
        public List<DeliveryChallanDetail> GetDeliveryChallanDetailListByDeliveryChallanID(Guid deliveryChallanID)
        {
            List<DeliveryChallanDetail> deliveryChallanDetailList = new List<DeliveryChallanDetail>();
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
                        cmd.CommandText = "[PSA].[GetDeliveryChallanDetailListByDeliveryChallanID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DelvChallanID", SqlDbType.UniqueIdentifier).Value = deliveryChallanID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    DeliveryChallanDetail deliveryChallanDetail = new DeliveryChallanDetail();
                                    {
                                        deliveryChallanDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : deliveryChallanDetail.ID);
                                        deliveryChallanDetail.DelvChallanID = (sdr["DelvChallanID"].ToString() != "" ? Guid.Parse(sdr["DelvChallanID"].ToString()) : deliveryChallanDetail.DelvChallanID);
                                        deliveryChallanDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : deliveryChallanDetail.ProductSpec);
                                        deliveryChallanDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty)
                                        };
                                        deliveryChallanDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        deliveryChallanDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        deliveryChallanDetail.ProductModel = new ProductModel();
                                        deliveryChallanDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        deliveryChallanDetail.ProductModel.Name = (sdr["ModelName"].ToString() != "" ? (sdr["ModelName"].ToString()) : deliveryChallanDetail.ProductModel.Name);
                                        deliveryChallanDetail.OrderQty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : deliveryChallanDetail.OrderQty);
                                        deliveryChallanDetail.DelvQty= (sdr["DelvQty"].ToString() != "" ? decimal.Parse(sdr["DelvQty"].ToString()) : deliveryChallanDetail.DelvQty);
                                        deliveryChallanDetail.Unit = new Unit();
                                        deliveryChallanDetail.Unit.Code = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : deliveryChallanDetail.Unit.Code);
                                        deliveryChallanDetail.Unit.Description = (sdr["Unit"].ToString() != "" ? (sdr["Unit"].ToString()) : deliveryChallanDetail.Unit.Description);

                                    }
                                    deliveryChallanDetailList.Add(deliveryChallanDetail);
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

            return deliveryChallanDetailList;
        }
        #endregion GetDeliveryChallanDetailListByDeliveryChallanID

        #region Insert Update DeliveryChallan
        public object InsertUpdateDeliveryChallan(DeliveryChallan deliveryChallan)
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
                        cmd.CommandText = "[PSA].[InsertUpdateDeliveryChallan]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = deliveryChallan.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = deliveryChallan.ID;
                        cmd.Parameters.Add("@DelvChallanNo", SqlDbType.VarChar, 20).Value = deliveryChallan.DelvChallanNo;
                        cmd.Parameters.Add("@DelvChallanRefNo", SqlDbType.VarChar, 20).Value = deliveryChallan.DelvChallanRefNo;
                        cmd.Parameters.Add("@DelvChallanDate", SqlDbType.DateTime).Value = deliveryChallan.DelvChallanDateFormatted;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = deliveryChallan.SaleOrderID;
                        cmd.Parameters.Add("@ProdOrderID", SqlDbType.UniqueIdentifier).Value = deliveryChallan.ProdOrderID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = deliveryChallan.CustomerID;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = deliveryChallan.PlantCode;
                        cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = deliveryChallan.PreparedBy;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = deliveryChallan.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = deliveryChallan.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = deliveryChallan.BranchCode;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = deliveryChallan.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = deliveryChallan.hdnFileID;
                        cmd.Parameters.Add("@VehiclePlateNo", SqlDbType.NVarChar, -1).Value = deliveryChallan.VehiclePlateNo;
                        cmd.Parameters.Add("@DriverName", SqlDbType.NVarChar, -1).Value = deliveryChallan.DriverName;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = deliveryChallan.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = deliveryChallan.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = deliveryChallan.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = deliveryChallan.PSASysCommon.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputProdOrderNo = cmd.Parameters.Add("@DeliveryChallanNoOut", SqlDbType.VarChar, 20);
                        outputProdOrderNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        deliveryChallan.ID = Guid.Parse(outputID.Value.ToString());
                        deliveryChallan.DelvChallanNo = outputProdOrderNo.Value.ToString();
                        return new
                        {
                            ID = deliveryChallan.ID,
                            DeliveryChallanNo = deliveryChallan.DelvChallanNo,
                            Status = outputStatus.Value.ToString(),
                            Message = deliveryChallan.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = deliveryChallan.ID,
                DeliveryChallanNo = deliveryChallan.DelvChallanNo,
                Status = outputStatus.Value.ToString(),
                Message = deliveryChallan.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update DeliveryChallan

        #region Delete DeliveryChallan
        public object DeleteDeliveryChallan(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteDeliveryChallan]";
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
        #endregion Delete DeliveryChallan

        #region Delete DeliveryChallan Detail
        public object DeleteDeliveryChallanDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteDeliveryChallanDetail]";
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
        #endregion Delete DeliveryChallan Detail

        #region GetDeliveryChallanForSelectListOnDemand
        public List<DeliveryChallan> GetDeliveryChallanForSelectListOnDemand(string searchTerm)
        {
            List<DeliveryChallan> deliveryChallanList = null;
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
                        cmd.CommandText = "[PSA].[GetDeliveryChallanForSelectListOnDemand]";
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
                                deliveryChallanList = new List<DeliveryChallan>();
                                while (sdr.Read())
                                {
                                    DeliveryChallan deliveryChallan = new DeliveryChallan();
                                    {
                                        deliveryChallan.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : deliveryChallan.ID);
                                        deliveryChallan.DelvChallanNo = (sdr["DelvChallanNo"].ToString() != "" ? sdr["DelvChallanNo"].ToString() : deliveryChallan.DelvChallanNo);
                                    }
                                    deliveryChallanList.Add(deliveryChallan);
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
            return deliveryChallanList;
        }
        #endregion GetDeliveryChallanForSelectListOnDemand

        #region GetDeliveryChallanForSelectList
        public List<DeliveryChallan> GetDeliveryChallanForSelectList(Guid? id)
        {
            List<DeliveryChallan> deliveryChallanList = null;
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
                        cmd.CommandText = "[PSA].[GetDeliveryChallanForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (id == null)
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
                                deliveryChallanList = new List<DeliveryChallan>();
                                while (sdr.Read())
                                {
                                    DeliveryChallan deliveryChallan = new DeliveryChallan();
                                    {
                                        deliveryChallan.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : deliveryChallan.ID);
                                        deliveryChallan.DelvChallanNo = (sdr["DelvChallanNo"].ToString() != "" ? sdr["DelvChallanNo"].ToString() : deliveryChallan.DelvChallanNo);
                                    }
                                    deliveryChallanList.Add(deliveryChallan);
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
            return deliveryChallanList;
        }
        #endregion GetDeliveryChallanForSelectList
    }
}

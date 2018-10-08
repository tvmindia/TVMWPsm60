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
    public class ProductionQCRepository: IProductionQCRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ProductionQCRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get All ProductionQC
        public List<ProductionQC> GetAllProductionQC(ProductionQCAdvanceSearch productionQCAdvanceSearch)
        {
            List<ProductionQC> productionQCList = null;
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
                        cmd.CommandText = "[PSA].[GetAllProductionQC]";
                        if (string.IsNullOrEmpty(productionQCAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = productionQCAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionQCAdvanceSearch.DataTablePaging.Start;
                        if (productionQCAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionQCAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = productionQCAdvanceSearch.AdvFromDate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = productionQCAdvanceSearch.AdvToDate;
                        if (productionQCAdvanceSearch.AdvCustomerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = productionQCAdvanceSearch.AdvCustomerID;
                        cmd.Parameters.Add("@AreaCode", SqlDbType.Int).Value = productionQCAdvanceSearch.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionQCAdvanceSearch.AdvBranchCode;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionQCAdvanceSearch.AdvDocumentStatusCode;
                        if (productionQCAdvanceSearch.AdvDocumentOwnerID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@DocumentOwnerID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionQCAdvanceSearch.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@ApprovalStatusCode", SqlDbType.Int).Value = productionQCAdvanceSearch.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.NVarChar).Value = productionQCAdvanceSearch.AdvEmailSentStatus;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = productionQCAdvanceSearch.AdvPlantCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionQCList = new List<ProductionQC>();
                                while (sdr.Read())
                                {
                                    ProductionQC productionQC = new ProductionQC();
                                    {
                                        productionQC.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionQC.ID);
                                        productionQC.ProdQCNo = (sdr["ProdQCNo"].ToString() != "" ? sdr["ProdQCNo"].ToString() : productionQC.ProdQCNo);
                                        productionQC.ProdQCDate = (sdr["ProdQCDate"].ToString() != "" ? DateTime.Parse(sdr["ProdQCDate"].ToString()) : productionQC.ProdQCDate);
                                        productionQC.ProdQCDateFormatted = (sdr["ProdQCDate"].ToString() != "" ? DateTime.Parse(sdr["ProdQCDate"].ToString()).ToString(_settings.DateFormat) : productionQC.ProdQCDateFormatted);
                                        productionQC.ProdOrderID= (sdr["ProdOrderID"].ToString() != "" ? Guid.Parse(sdr["ProdOrderID"].ToString()) : productionQC.ProdOrderID);
                                        productionQC.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionQC.ProdOrderNo);

                                        productionQC.PlantCode= (sdr["PlantCode"].ToString() != "" ? int.Parse(sdr["PlantCode"].ToString()) : productionQC.PlantCode);
                                        productionQC.Plant = new Plant();
                                        productionQC.Plant.Code= (sdr["PlantCode"].ToString() != "" ? int.Parse(sdr["PlantCode"].ToString()) : productionQC.Plant.Code);
                                        productionQC.Plant.Description= (sdr["PlantDescription"].ToString() != "" ? (sdr["PlantDescription"].ToString()) : productionQC.Plant.Description);
                                        productionQC.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionQC.CustomerID);
                                        productionQC.Customer = new Customer();
                                        productionQC.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionQC.Customer.ID);
                                        productionQC.Customer.CompanyName = (sdr["CustomerCompanyName"].ToString() != "" ? sdr["CustomerCompanyName"].ToString() : productionQC.Customer.CompanyName);
                                        productionQC.Customer.ContactPerson = (sdr["CustomerContactPerson"].ToString() != "" ? sdr["CustomerContactPerson"].ToString() : productionQC.Customer.ContactPerson);
                                        productionQC.Customer.Mobile = (sdr["CustomerMobile"].ToString() != "" ? sdr["CustomerMobile"].ToString() : productionQC.Customer.Mobile);
                                        productionQC.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionQC.DocumentStatusCode);
                                        productionQC.DocumentStatus = new DocumentStatus();
                                        productionQC.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionQC.DocumentStatus.Code);
                                        productionQC.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : productionQC.DocumentStatus.Description);
                                        productionQC.PreparedBy= (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : productionQC.PreparedBy);
                                        productionQC.Employee = new Employee();
                                        productionQC.Employee.ID= (sdr["PreparedBy"].ToString() != "" ? Guid.Parse(sdr["PreparedBy"].ToString()) : productionQC.Employee.ID);
                                        productionQC.Employee.Name= (sdr["EmployeeName"].ToString() != "" ? (sdr["EmployeeName"].ToString()) : productionQC.Employee.Name);
                                        productionQC.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionQC.GeneralNotes);
                                        productionQC.LatestApprovalStatus= (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : productionQC.LatestApprovalStatus);
                                        productionQC.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : productionQC.DocumentOwnerID);
                                        productionQC.Branch = new Branch();
                                        productionQC.Branch.Description = (sdr["BranchDescription"].ToString() != "" ? sdr["BranchDescription"].ToString() : productionQC.Branch.Description);
                                        productionQC.Branch.Code = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) :  productionQC.Branch.Code );
                                        productionQC.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productionQC.FilteredCount);
                                        productionQC.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionQC.FilteredCount);
                                        productionQC.ApprovalStatus = new ApprovalStatus();
                                        productionQC.ApprovalStatus.Description = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : productionQC.ApprovalStatus.Description);
                                        productionQC.PSAUser = new PSAUser();
                                        productionQC.PSAUser.LoginName = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : productionQC.PSAUser.LoginName);
                                        productionQC.EmailSentYN = (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : productionQC.EmailSentYN);
                                        productionQC.Area = new Area();
                                        productionQC.Area.Description = (sdr["Area"].ToString() != "" ? sdr["Area"].ToString() : productionQC.Area.Description);
                                        productionQC.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString():productionQC.ProdOrderNo);
                                    }
                                    productionQCList.Add(productionQC);
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

            return productionQCList;
        }
        #endregion Get All ProductionQC
        #region Get ProductionQC
        public ProductionQC GetProductionQC(Guid id)
        {
            ProductionQC productionQC = new ProductionQC();
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
                        cmd.CommandText = "[PSA].[GetProductionQC]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    productionQC.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionQC.ID);
                                    productionQC.ProdOrderID= (sdr["ProdOrderID"].ToString() != "" ? Guid.Parse(sdr["ProdOrderID"].ToString()) : productionQC.ID);
                                    productionQC.ProdQCNo = (sdr["ProdQCNo"].ToString() != "" ? sdr["ProdQCNo"].ToString() : productionQC.ProdQCNo);
                                    productionQC.ProdQCDate = (sdr["ProdQCDate"].ToString() != "" ? DateTime.Parse(sdr["ProdQCDate"].ToString()) : productionQC.ProdQCDate);
                                    productionQC.ProdQCDateFormatted = (sdr["ProdQCDate"].ToString() != "" ? DateTime.Parse(sdr["ProdQCDate"].ToString()).ToString("dd-MMM-yyyy") : productionQC.ProdQCDateFormatted);
                                    productionQC.PlantCode= (sdr["PlantCode"].ToString() != "" ? int.Parse(sdr["PlantCode"].ToString()) : productionQC.PlantCode);
                                    productionQC.ProdQCRefNo= (sdr["ProdQCRefNo"].ToString() != "" ? sdr["ProdQCRefNo"].ToString() : productionQC.ProdQCRefNo);
                                    productionQC.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionQC.CustomerID);
                                    productionQC.DocumentStatusCode = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionQC.DocumentStatusCode);
                                    productionQC.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionQC.GeneralNotes);
                                    productionQC.DocumentOwnerID = (sdr["DocumentOwnerID"].ToString() != "" ? Guid.Parse(sdr["DocumentOwnerID"].ToString()) : productionQC.DocumentOwnerID);
                                    productionQC.BranchCode = (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : productionQC.BranchCode);
                                    productionQC.EmailSentYN= (sdr["EmailSentYN"].ToString() != "" ? bool.Parse(sdr["EmailSentYN"].ToString()) : productionQC.EmailSentYN);
                                    productionQC.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? int.Parse(sdr["LatestApprovalStatus"].ToString()) : productionQC.LatestApprovalStatus);
                                    productionQC.LatestApprovalStatusDescription = (sdr["LatestApprovalStatusDescription"].ToString() != "" ? (sdr["LatestApprovalStatusDescription"].ToString()) : productionQC.LatestApprovalStatusDescription);
                                    productionQC.DocumentStatus = new DocumentStatus();
                                    productionQC.DocumentStatus.Code = (sdr["DocumentStatusCode"].ToString() != "" ? int.Parse(sdr["DocumentStatusCode"].ToString()) : productionQC.DocumentStatus.Code);
                                    productionQC.DocumentStatus.Description = (sdr["DocumentStatusDescription"].ToString() != "" ? (sdr["DocumentStatusDescription"].ToString()) : productionQC.DocumentStatus.Description);
                                    productionQC.DocumentOwners = (sdr["DocumentOwners"].ToString() != "" ? (sdr["DocumentOwners"].ToString()).Split(',') : productionQC.DocumentOwners);
                                    productionQC.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : productionQC.DocumentOwner);
                                    productionQC.Branch = new Branch();
                                    productionQC.Branch.Description = (sdr["Branch"].ToString() != "" ? sdr["Branch"].ToString() : productionQC.Branch.Description);
                                    productionQC.Branch.Code= (sdr["BranchCode"].ToString() != "" ? int.Parse(sdr["BranchCode"].ToString()) : productionQC.Branch.Code);
                                    productionQC.Customer = new Customer();
                                    productionQC.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : productionQC.Customer.CompanyName);
                                    productionQC.Customer.ID= (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : productionQC.Customer.ID);
                                    productionQC.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? sdr["ProdOrderNo"].ToString() : productionQC.ProdOrderNo);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return productionQC;
        }
        #endregion Get ProductionQC
        #region GetAllProductionQC Details
        public List<ProductionQCDetail> GetProductionQCDetailListByProductionQCID(Guid productionQCID)
        {
            List<ProductionQCDetail> productionQCDetailList = new List<ProductionQCDetail>();
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
                        cmd.CommandText = "[PSA].[GetProductionQCDetailListByProductionQCID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ProdQCID", SqlDbType.UniqueIdentifier).Value = productionQCID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    ProductionQCDetail productionQCDetail = new ProductionQCDetail();
                                    {
                                        productionQCDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionQCDetail.ID);
                                        productionQCDetail.ProdQCID = (sdr["ProdQCID"].ToString() != "" ? Guid.Parse(sdr["ProdQCID"].ToString()) : productionQCDetail.ProdQCID);
                                        productionQCDetail.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : productionQCDetail.ProductSpec);
                                        productionQCDetail.Product = new Product()
                                        {
                                            ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty),
                                            Code = (sdr["ProductCode"].ToString() != "" ? sdr["ProductCode"].ToString() : string.Empty),
                                            Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : string.Empty),
                                            HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : String.Empty)
                                        };
                                        productionQCDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                        productionQCDetail.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        productionQCDetail.ProductModel = new ProductModel();
                                        productionQCDetail.ProductModel.ID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : Guid.Empty);
                                        productionQCDetail.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? (sdr["ProductModelName"].ToString()) : productionQCDetail.ProductModel.Name);
                                        productionQCDetail.QCQty = (sdr["QCQty"].ToString() != "" ? decimal.Parse(sdr["QCQty"].ToString()) : productionQCDetail.QCQty);
                                        productionQCDetail.QCBy = (sdr["QCBy"].ToString() != "" ? Guid.Parse(sdr["QCBy"].ToString()) : productionQCDetail.QCBy);
                                        productionQCDetail.QCDate = (sdr["QCDate"].ToString() != "" ? DateTime.Parse(sdr["QCDate"].ToString()) : productionQCDetail.QCDate);
                                        productionQCDetail.QCDateFormatted = (sdr["QCDate"].ToString() != "" ? DateTime.Parse(sdr["QCDate"].ToString()).ToString("dd-MMM-yyyy") : productionQCDetail.QCDateFormatted);
                                        productionQCDetail.ProducedQty = (sdr["ProducedQty"].ToString() != "" ? decimal.Parse(sdr["ProducedQty"].ToString()) : productionQCDetail.ProducedQty);
                                        productionQCDetail.QCQtyPrevious = (sdr["QCQtyPrevious"].ToString() != "" ? decimal.Parse(sdr["QCQtyPrevious"].ToString()) : productionQCDetail.QCQtyPrevious);
                                        productionQCDetail.Employee = new Employee();
                                        productionQCDetail.Employee.Name= (sdr["EmployeeName"].ToString() != "" ? (sdr["EmployeeName"].ToString()) : productionQCDetail.Employee.Name);
                                        productionQCDetail.Unit = new Unit();
                                        productionQCDetail.Unit.Description = (sdr["UnitDescription"].ToString() != "" ? (sdr["UnitDescription"].ToString()) : productionQCDetail.Unit.Description);
                                    }
                                    productionQCDetailList.Add(productionQCDetail);
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

            return productionQCDetailList;
        }


        #endregion GetAllProductionQC Details
        #region Insert Update ProductionQC
        public object InsertUpdateProductionQC(ProductionQC productionQC)
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
                        cmd.CommandText = "[PSA].[InsertUpdateProductionQC]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = productionQC.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = productionQC.ID;
                        cmd.Parameters.Add("@ProdOrderID",SqlDbType.UniqueIdentifier).Value = productionQC.ProdOrderID;
                        cmd.Parameters.Add("@ProdQCNo", SqlDbType.VarChar, 20).Value = productionQC.ProdQCNo;
                        cmd.Parameters.Add("@ProdQCRefNo", SqlDbType.VarChar, 20).Value = productionQC.ProdQCRefNo;
                        cmd.Parameters.Add("@ProdQCDate", SqlDbType.DateTime).Value = productionQC.ProdQCDateFormatted;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = productionQC.CustomerID;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = productionQC.PlantCode;
                        cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = productionQC.PreparedBy;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionQC.DocumentStatusCode;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = productionQC.DetailXML;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = productionQC.hdnFileID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = productionQC.GeneralNotes;
                        cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionQC.DocumentOwnerID;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionQC.BranchCode;
                        //-----------------------//
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = productionQC.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = productionQC.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = productionQC.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = productionQC.PSASysCommon.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@StatusOut", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputProdOrderNo = cmd.Parameters.Add("@ProdQCNoOut", SqlDbType.VarChar, 20);
                        outputProdOrderNo.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        productionQC.ID = Guid.Parse(outputID.Value.ToString());
                        productionQC.ProdQCNo = outputProdOrderNo.Value.ToString();
                        return new
                        {
                            ID = productionQC.ID,
                            ProdQCNo = productionQC.ProdQCNo,
                            Status = outputStatus.Value.ToString(),
                            Message = productionQC.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = productionQC.ID,
                ProdQCNo = productionQC.ProdQCNo,
                Status = outputStatus.Value.ToString(),
                Message = productionQC.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion Insert Update ProductionQC
        #region Delete ProductionQC
        public object DeleteProductionQC(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteProductionQC]";
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
        #endregion Delete ProductionQC
        #region Delete ProductionQC Detail
        public object DeleteProductionQCDetail(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteProductionQCDetail]";
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
        #endregion Delete ProductionQC Detail
    }
}

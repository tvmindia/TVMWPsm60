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
    public class ProductModelRepository : IProductModelRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ProductModelRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region InsertUpdateProductModel
        public object InsertUpdateProductModel(ProductModel productModel)
        {
            SqlParameter outputStatus, outputID = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateProductModel]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = productModel.IsUpdate;
                        if (productModel.ID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = productModel.ID;
                        }
                        cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productModel.ProductID;
                        cmd.Parameters.Add("Name", SqlDbType.VarChar).Value = productModel.Name;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.Int).Value = productModel.UnitCode;
                        cmd.Parameters.Add("@Specification", SqlDbType.NVarChar).Value = productModel.Specification;
                        cmd.Parameters.Add("@CostPrice", SqlDbType.Decimal).Value = productModel.CostPrice;
                        cmd.Parameters.Add("@SellingPrice", SqlDbType.Decimal).Value = productModel.SellingPrice;
                        cmd.Parameters.Add("@IntroducedDate", SqlDbType.DateTime).Value = productModel.IntroducedDateFormatted;
                        cmd.Parameters.Add("StockQty", SqlDbType.Decimal).Value = productModel.StockQty;
                        cmd.Parameters.Add("@ImageURL", SqlDbType.NVarChar).Value = productModel.ImageURL;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = productModel.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = productModel.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = productModel.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = productModel.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(productModel.IsUpdate ? _appConstant.UpdateFailure : _appConstant.InsertFailure);
                    case "1":
                        productModel.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            Code = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = productModel.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = productModel.ID,
                Status = outputStatus.Value.ToString(),
                Message = productModel.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateProductModel

        #region GetAllProductModel
        public List<ProductModel> GetAllProductModel(ProductModelAdvanceSearch productModelAdvanceSearch)
        {
            List<ProductModel> productModelList = null;
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
                        cmd.CommandText = "[PSA].[GetAllProductModel]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productModelAdvanceSearch.SearchTerm) ? "" : productModelAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productModelAdvanceSearch.DataTablePaging.Start;
                        if (productModelAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productModelAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productModelList = new List<ProductModel>();
                                while (sdr.Read())
                                {
                                    ProductModel productModel = new ProductModel();
                                    {
                                        productModel.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productModel.ID);
                                        productModel.Product = new Product();  
                                        productModel.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : productModel.Product.Name);
                                       // productModel.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : productModel.ProductID);
                                        productModel.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : productModel.Name);
                                        productModel.Unit = new Unit();
                                        productModel.Unit.Description = (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : productModel.Unit.Description);
                                       // productModel.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : productModel.UnitCode);
                                        productModel.Specification = (sdr["Specification"].ToString() != "" ? sdr["Specification"].ToString() : productModel.Specification);
                                        productModel.CostPrice = (sdr["CostPrice"].ToString() != "" ? Decimal.Parse(sdr["CostPrice"].ToString()) : productModel.CostPrice);
                                        productModel.SellingPrice = (sdr["SellingPrice"].ToString() != "" ? Decimal.Parse(sdr["SellingPrice"].ToString()) : productModel.SellingPrice);                                       
                                        productModel.IntroducedDate = (sdr["IntroducedDate"].ToString() != "" ? DateTime.Parse(sdr["IntroducedDate"].ToString()) : productModel.IntroducedDate);
                                        productModel.IntroducedDateFormatted = (sdr["IntroducedDate"].ToString() != "" ? DateTime.Parse(sdr["IntroducedDate"].ToString()).ToString(_settings.DateFormat) : productModel.IntroducedDateFormatted);
                                        productModel.StockQty = (sdr["StockQty"].ToString() != "" ? Decimal.Parse(sdr["StockQty"].ToString()) : productModel.StockQty);
                                        productModel.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productModel.TotalCount);
                                        productModel.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productModel.FilteredCount);
                                    }
                                    productModelList.Add(productModel);
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
            return productModelList;
        }
        #endregion GetAllProductModel

        #region GetProductModel
        public ProductModel GetProductModel(Guid id)
        {
            ProductModel productModel = null;
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
                        cmd.CommandText = "[PSA].[GetProductModel]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    productModel = new ProductModel();
                                    productModel.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productModel.ID);
                                    productModel.Product = new Product();
                                    productModel.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : productModel.Product.Name);
                                    productModel.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : productModel.ProductID);
                                    productModel.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : productModel.Name);
                                    productModel.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : productModel.UnitCode);
                                    productModel.Specification = (sdr["Specification"].ToString() != "" ? sdr["Specification"].ToString() : productModel.Specification);
                                    productModel.CostPrice = (sdr["CostPrice"].ToString() != "" ? Decimal.Parse(sdr["CostPrice"].ToString()) : productModel.CostPrice);
                                    productModel.SellingPrice = (sdr["SellingPrice"].ToString() != "" ? Decimal.Parse(sdr["SellingPrice"].ToString()) : productModel.SellingPrice);
                                    productModel.IntroducedDate = (sdr["IntroducedDate"].ToString() != "" ? DateTime.Parse(sdr["IntroducedDate"].ToString()) : productModel.IntroducedDate);
                                    productModel.IntroducedDateFormatted = (sdr["IntroducedDate"].ToString() != "" ? DateTime.Parse(sdr["IntroducedDate"].ToString()).ToString(_settings.DateFormat) : productModel.IntroducedDateFormatted);
                                    productModel.StockQty = (sdr["StockQty"].ToString() != "" ? Decimal.Parse(sdr["StockQty"].ToString()) : productModel.StockQty);
                                    productModel.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : productModel.ImageURL);
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
            return productModel;
        }
        #endregion GetProductModel

        #region CheckProductModelNameExist
        public bool CheckProductModelNameExist(ProductModel productModel)
        {
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
                        cmd.CommandText = "[PSA].[CheckProductModelNameExist]";                        
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = productModel.ID;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = productModel.Name;
                        cmd.CommandType = CommandType.StoredProcedure;
                        Object res = cmd.ExecuteScalar();
                        return (res.ToString() == "Exists" ? true : false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion CheckProductModelNameExist

        #region DeleteProductModel
        public object DeleteProductModel(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteProductModel]";
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
        #endregion

        #region GetProductModelForSelectList
        public List<ProductModel> GetProductModelForSelectList(Guid productID)
        {
            List<ProductModel> productModelList = null;
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
                        cmd.CommandText = "[PSA].[GetProductModelForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if(productID!=Guid.Empty)
                        cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productModelList = new List<ProductModel>();
                                while (sdr.Read())
                                {
                                    ProductModel productModel = new ProductModel();
                                    {
                                        productModel.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productModel.ID);
                                        productModel.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : productModel.Name);
                                    }
                                    productModelList.Add(productModel);
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
            return productModelList;
        }
        #endregion
    }
}

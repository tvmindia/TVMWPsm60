using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PilotSmithApp.RepositoryService.Service
{
    public class ProductCategoryRepository:IProductCategoryRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
       // public ConnectionState Connectionstate { get; private set; }

        public ProductCategoryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region InsertUpateProductCategory
        public object InsertUpdateProductCategory(ProductCategory productCategory)
        {
            SqlParameter outputStatus, outputCode = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[InsertUpdateProductCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = productCategory.IsUpdate;
                        if (productCategory.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = productCategory.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = productCategory.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = productCategory.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = productCategory.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = productCategory.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = productCategory.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 5);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(productCategory.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        productCategory.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = productCategory.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = outputCode.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = productCategory.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllProductCategory
        public List<ProductCategory> GetAllProductCategory(ProductCategoryAdvanceSearch productCategoryAdvanceSearch)
        {
            List<ProductCategory> productCategoryList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[PSA].[GetAllProductCategory]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productCategoryAdvanceSearch.SearchTerm)?"": productCategoryAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productCategoryAdvanceSearch.DataTablePaging.Start;
                        if (productCategoryAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productCategoryAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if((sdr!=null)&&(sdr.HasRows))
                            {
                                productCategoryList = new List<ProductCategory>();
                                while(sdr.Read())
                                {
                                    ProductCategory productCategory = new ProductCategory();
                                    {
                                        productCategory.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : productCategory.Code);
                                        productCategory.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : productCategory.Description);
                                        productCategory.PSASysCommon = new PSASysCommon();
                                        productCategory.PSASysCommon.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : productCategory.PSASysCommon.CreatedDateString);
                                        productCategory.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productCategory.TotalCount);
                                        productCategory.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productCategory.FilteredCount);
                                    }
                                    productCategoryList.Add(productCategory);
                                }
                                
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return productCategoryList;
        }
        #endregion

        #region GetProductCategory
        public ProductCategory GetProductCategory(int code)
        {
            ProductCategory productCategory = null;
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
                        cmd.CommandText = "[PSA].[GetProductCategory]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    productCategory = new ProductCategory();
                                    productCategory.Code= (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : productCategory.Code);
                                    productCategory.Description= (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : productCategory.Description);
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
            return productCategory;
        }
        #endregion

        #region CheckProductCategoryExist
        public bool CheckProductCategoryExist(ProductCategory productCategory)
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
                        cmd.CommandText = "[PSA].[CheckProductCategoryExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = productCategory.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = productCategory.Description;
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
        #endregion

        #region DeleteProductCategory
        public object DeleteProductCategory(int code)
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
                        cmd.CommandText = "[PSA].[DeleteProductCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConst.DeleteFailure);

                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.DeleteSuccess
            };
        }
        #endregion

        #region GetProductCategoryForSelectList
        public List<ProductCategory> GetProductCategoryForSelectList()
        {
            List<ProductCategory> productCategoryList = null;
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
                        cmd.CommandText = "[PSA].[GetProductCategoryForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productCategoryList = new List<ProductCategory>();
                                while(sdr.Read())
                                {
                                    ProductCategory productCategory = new ProductCategory();
                                    {
                                        productCategory.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : productCategory.Code);
                                        productCategory.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : productCategory.Description);
                                    }
                                    productCategoryList.Add(productCategory);
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
            return productCategoryList;
        }
        #endregion
    }
}

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
    public class ProductRepository:IProductRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ProductRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateProduct
        public object InsertUpdateProduct(Product product)
        {
            SqlParameter outputStatus, outputID = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateProduct]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = product.IsUpdate;
                        if(product.ID==Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = product.ID;
                        }
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = product.Code.Replace("-", "_ ");
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = product.Name.Replace("-","_ ");
                        cmd.Parameters.Add("@TallyName", SqlDbType.VarChar).Value = product.TallyName;
                        cmd.Parameters.Add("@ProductCategoryCode", SqlDbType.Int).Value = product.ProductCategoryCode;
                        cmd.Parameters.Add("@IntroducedDate", SqlDbType.DateTime).Value = product.IntroducedDateFormatted;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.UniqueIdentifier).Value = product.CompanyID;
                        cmd.Parameters.Add("@HSNCode", SqlDbType.VarChar).Value = product.HSNCode;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = product.hdnPopupFileID;
                        cmd.Parameters.Add("@Purpose", SqlDbType.NVarChar, 255).Value = product.Purpose;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = product.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = product.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = product.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = product.PSASysCommon.UpdatedDate;
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
                        throw new Exception(product.IsUpdate ? _appConstant.UpdateFailure : _appConstant.InsertFailure);
                    case "1":
                        product.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            Code = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = product.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
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
                ID = product.ID,
                Status = outputStatus.Value.ToString(),
                Message = product.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateProduct

        #region GetAllProduct
        public List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch)
        {
            List<Product> productList = null;
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
                        cmd.CommandText = "[PSA].[GetAllProduct]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productAdvanceSearch.SearchTerm) ? "" : productAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productAdvanceSearch.DataTablePaging.Start;
                        if (productAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productList = new List<Product>();
                                while(sdr.Read())
                                {
                                    Product product = new Product();
                                    {
                                        product.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : product.ID);
                                        product.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : product.Code);
                                        product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : product.Name);
                                        product.ProductCategory = new ProductCategory();
                                        product.ProductCategory.Description = (sdr["Category"].ToString() != "" ? (sdr["Category"].ToString()) : product.ProductCategory.Description);
                                        product.IntroducedDate = (sdr["IntroducedDate"].ToString() != "" ? DateTime.Parse(sdr["IntroducedDate"].ToString()) : product.IntroducedDate);
                                        product.IntroducedDateFormatted = (sdr["IntroducedDate"].ToString() != "" ? DateTime.Parse(sdr["IntroducedDate"].ToString()).ToString(_settings.DateFormat) : product.IntroducedDateFormatted);
                                        product.Company = new Company();
                                        product.Company.Name = (sdr["CompanyName"].ToString() != "" ? (sdr["CompanyName"].ToString()) : product.Company.Name);
                                        product.HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : product.HSNCode);
                                        product.Purpose = (sdr["Purpose"].ToString() != "" ? sdr["Purpose"].ToString() : product.Purpose);
                                        product.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : product.TotalCount);
                                        product.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : product.FilteredCount);
                                    }
                                    productList.Add(product);
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
            return productList;
        }
        #endregion GetAllProduct

        #region GetProduct
        public Product GetProduct(Guid id)
        {
            Product product = null;
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
                        cmd.CommandText = "[PSA].[GetProduct]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    product = new Product();
                                    product.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : product.ID);
                                    product.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : product.Code);
                                    product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : product.Name);
                                    product.TallyName = (sdr["TallyName"].ToString() != "" ? sdr["TallyName"].ToString() : product.TallyName);
                                    product.ProductCategoryCode = (sdr["ProductCategoryCode"].ToString() != "" ? int.Parse(sdr["ProductCategoryCode"].ToString()) : product.ProductCategoryCode);
                                    product.ProductCategory = new ProductCategory();
                                    product.ProductCategory.Description = (sdr["Category"].ToString() != "" ? (sdr["Category"].ToString()) : product.ProductCategory.Description);
                                    product.IntroducedDateFormatted = (sdr["IntroducedDate"].ToString() != "" ? DateTime.Parse(sdr["IntroducedDate"].ToString()).ToString(_settings.DateFormat) : product.IntroducedDateFormatted);
                                    product.CompanyID = (sdr["CompanyID"].ToString() != "" ? Guid.Parse(sdr["CompanyID"].ToString()) : product.CompanyID);
                                    product.Company = new Company();
                                    product.Company.Name = (sdr["CompanyName"].ToString() != "" ? (sdr["CompanyName"].ToString()) : product.Company.Name);
                                    product.HSNCode = (sdr["HSNCode"].ToString() != "" ? sdr["HSNCode"].ToString() : product.HSNCode);
                                    product.Purpose = (sdr["Purpose"].ToString() != "" ? sdr["Purpose"].ToString() : product.Purpose);
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
            return product;
        }
        #endregion GetProduct

        #region CheckProductCodeExist
        public bool CheckProductCodeExist(Product product)
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
                        cmd.CommandText = "[PSA].[CheckProductCodeExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = product.Code;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = product.ID;
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
        #endregion CheckProductCodeExist

        #region DeleteProduct
        public object DeleteProduct(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteProduct]";
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

        #region GetProductForSelectList
        public List<Product> GetProductForSelectList()
        {
            List<Product> productList = null;
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
                        cmd.CommandText = "[PSA].[GetProductForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productList = new List<Product>();
                                while (sdr.Read())
                                {
                                    Product product = new Product();
                                    {
                                        product.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : product.ID);
                                        product.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : product.Code);
                                        product.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : product.Name);
                                    }
                                    productList.Add(product);
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
            return productList;
        }
        #endregion

        #region GetProductCode
        public string GetProductCode()
        {
            SqlParameter outputCode = null;
            string code;
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
                        cmd.CommandText = "[PSA].[GetProductCode]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 50);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        code = outputCode.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return code;
        }
        #endregion GetProductCode

    }
}

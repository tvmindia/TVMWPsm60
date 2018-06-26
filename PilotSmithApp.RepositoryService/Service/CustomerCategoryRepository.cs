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
    public class CustomerCategoryRepository:ICustomerCategoryRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        // public ConnectionState Connectionstate { get; private set; }

        public CustomerCategoryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateCustomerCategory
        public object InsertUpdateCustomerCategory(CustomerCategory customerCategory)
        {
            SqlParameter outputStatus, outputCode = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateCustomerCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = customerCategory.IsUpdate;
                        if (customerCategory.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = customerCategory.Code;

                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = customerCategory.Name;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 250).Value = customerCategory.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = customerCategory.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 250).Value = customerCategory.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customerCategory.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.Int);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(customerCategory.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        customerCategory.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = customerCategory.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = outputCode.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = customerCategory.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllCustomerCategory
        public List<CustomerCategory> GetAllCustomerCategory(CustomerCategoryAdvanceSearch customerCategoryAdvanceSearch)
        {
            List<CustomerCategory> customerCategoryList = null;
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
                        cmd.CommandText = "[PSA].[GetAllCustomerCategory]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(customerCategoryAdvanceSearch.SearchTerm) ? "" : customerCategoryAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = customerCategoryAdvanceSearch.DataTablePaging.Start;
                        if (customerCategoryAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = customerCategoryAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerCategoryList = new List<CustomerCategory>();
                                while (sdr.Read())
                                {
                                    CustomerCategory customerCategory = new CustomerCategory();
                                    {
                                        customerCategory.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : customerCategory.Code);
                                        customerCategory.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : customerCategory.Name);
                                        customerCategory.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : customerCategory.TotalCount);
                                        customerCategory.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : customerCategory.FilteredCount);
                                    }
                                    customerCategoryList.Add(customerCategory);
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
            return customerCategoryList;
        }
        #endregion

        #region GetCustomerCategory
        public CustomerCategory GetCustomerCategory(int code)
        {
            CustomerCategory customerCategory = null;
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
                        cmd.CommandText = "[PSA].[GetCustomerCategory]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    customerCategory = new CustomerCategory();
                                    customerCategory.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : customerCategory.Code);
                                    customerCategory.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : customerCategory.Name);
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
            return customerCategory;
        }
        #endregion

        #region CheckCustomerCategoryNameExist
        public bool CheckCustomerCategoryNameExist(CustomerCategory customerCategory)
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
                        cmd.CommandText = "[PSA].[CheckCustomerCategoryNameExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = customerCategory.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, -1).Value = customerCategory.Name;
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

        #region DeleteCustomerCategory
        public object DeleteCustomerCategory(int code)
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
                        cmd.CommandText = "[PSA].[DeleteCustomerCategory]";
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
            catch (Exception ex)
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

        #region GetCustomerCategoryForSelectList
        public List<CustomerCategory> GetCustomerCategoryForSelectList()
        {
            List<CustomerCategory> customertCategoryList = null;
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
                        cmd.CommandText = "[PSA].[GetCustomerCategoryForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customertCategoryList = new List<CustomerCategory>();
                                while (sdr.Read())
                                {
                                    CustomerCategory customerCategory = new CustomerCategory();
                                    {
                                        customerCategory.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : customerCategory.Code);
                                        customerCategory.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : customerCategory.Name);
                                    }
                                    customertCategoryList.Add(customerCategory);
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
            return customertCategoryList;
        }
        #endregion GetCustomerCategoryForSelectList

    }
}

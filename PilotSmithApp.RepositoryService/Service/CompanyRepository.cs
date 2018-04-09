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
    public class CompanyRepository : ICompanyRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CompanyRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateCompany
        public object InsertUpdateCompany(Company company)
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
                        cmd.CommandText = "[PSA].[InsertUpdateCompany]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = company.IsUpdate;
                        if (company.ID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = company.ID;
                        }
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = company.Name;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = company.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = company.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = company.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = company.PSASysCommon.UpdatedDate;
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
                        throw new Exception(company.IsUpdate ? _appConstant.UpdateFailure : _appConstant.InsertFailure);
                    case "1":
                        company.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            Code = outputID.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = company.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                ID = company.ID,
                Status = outputStatus.Value.ToString(),
                Message = company.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateCompany

        #region CheckCompanyNameExist
        public bool CheckCompanyNameExist(Company company)
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
                        cmd.CommandText = "[PSA].[CheckCompanyNameExist]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = company.ID;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = company.Name;
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
        #endregion CheckCompanyNameExist

        #region GetAllCompany
        public List<Company> GetAllCompany(CompanyAdvanceSearch companyAdvanceSearch)
        {
            List<Company> companyList = null;
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
                        cmd.CommandText = "[PSA].[GetAllCompany]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(companyAdvanceSearch.SearchTerm) ? "" : companyAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = companyAdvanceSearch.DataTablePaging.Start;
                        if (companyAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = companyAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                companyList = new List<Company>();
                                while (sdr.Read())
                                {
                                    Company company = new Company();
                                    {
                                        company.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : company.ID);
                                        company.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : company.Name);
                                        company.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : company.TotalCount);
                                        company.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : company.FilteredCount);
                                    }
                                    companyList.Add(company);
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
            return companyList;
        }
        #endregion GetAllCompany

        #region GetCompany
        public Company GetCompany(Guid id)
        {
            Company company = null;
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
                        cmd.CommandText = "[PSA].[GetCompany]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    company = new Company();
                                    company.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : company.ID);
                                    company.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : company.Name);
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
            return company;
        }
        #endregion GetCompany

        #region DeleteCompany
        public object DeleteCompany(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteCompany]";
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
        #endregion DeleteCompany

        #region GetCompanyForSelectList
        public List<Company> GetCompanyForSelectList()
        {
            List<Company> companyList = null;
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
                        cmd.CommandText = "[PSA].[GetCompanyForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                companyList = new List<Company>();
                                while (sdr.Read())
                                {
                                    Company company = new Company();
                                    {
                                        company.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : company.ID);                                       
                                        company.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : company.Name);
                                    }
                                    companyList.Add(company);
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
            return companyList;
        }
        #endregion GetCompanyForSelectList

    }
}

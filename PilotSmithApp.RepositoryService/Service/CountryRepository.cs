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
    public class CountryRepository:ICountryRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();
        Settings _settings = new Settings();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CountryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateCountry
        public object InsertUpdateCountry(Country Country)
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
                        cmd.CommandText = "[PSA].[InsertUpdateCountry]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = Country.IsUpdate;
                        if (Country.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = Country.Code;

                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = Country.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 250).Value = Country.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Country.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 250).Value = Country.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = Country.PSASysCommon.UpdatedDate;
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
                        throw new Exception(Country.IsUpdate ? _appConstant.UpdateFailure : _appConstant.InsertFailure);
                    case "1":
                        Country.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = Country.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
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
                Message = Country.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion

        #region GetAllCountry
        public List<Country> GetAllCountry(CountryAdvanceSearch CountryAdvanceSearch)
        {
            List<Country> CountryList = null;
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
                        cmd.CommandText = "[PSA].[GetAllCountry]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(CountryAdvanceSearch.SearchTerm) ? "" : CountryAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = CountryAdvanceSearch.DataTablePaging.Start;
                        if (CountryAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = CountryAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CountryList = new List<Country>();
                                while (sdr.Read())
                                {
                                    Country Country = new Country();
                                    {
                                        Country.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : Country.Code);
                                        Country.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : Country.Description);
                                        Country.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : Country.TotalCount);
                                        Country.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : Country.FilteredCount);
                                    }
                                    CountryList.Add(Country);
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
            return CountryList;
        }
        #endregion

        #region GetCountry
        public Country GetCountry(int code)
        {
            Country Country = null;
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
                        cmd.CommandText = "[PSA].[GetCountry]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    Country = new Country();
                                    Country.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : Country.Code);
                                    Country.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : Country.Description);
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
            return Country;
        }
        #endregion

        #region CheckCountryExist
        public bool CheckCountryExist(Country Country)
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
                        cmd.CommandText = "[PSA].[CheckCountryExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = Country.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = Country.Description;
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

        #region DeleteCountry
        public object DeleteCountry(int code)
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
                        cmd.CommandText = "[PSA].[DeleteCountry]";
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

        #region Get Country SelectList
        public List<Country> GetCountryForSelectList()
        {
            List<Country> CountryList = null;
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
                        cmd.CommandText = "[PSA].[GetCountryForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CountryList = new List<Country>();
                                while (sdr.Read())
                                {
                                    Country Country = new Country();
                                    {
                                        Country.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : Country.Code);
                                        Country.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : Country.Description);
                                    }
                                    CountryList.Add(Country);
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
            return CountryList;
        }
        #endregion Get Country SelectList
    }
}

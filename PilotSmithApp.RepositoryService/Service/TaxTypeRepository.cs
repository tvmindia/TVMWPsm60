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
    public class TaxTypeRepository:ITaxTypeRepository
    {

        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public TaxTypeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region InsertUpdateTaxType
        public object InsertUpdateTaxType(TaxType taxType)
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
                        cmd.CommandText = "[PSA].[InsertUpdateTaxType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = taxType.IsUpdate;
                        if (taxType.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = taxType.Code;

                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = taxType.Description;
                        cmd.Parameters.Add("@CGSTPercentage", SqlDbType.Decimal).Value = taxType.CGSTPercentage;
                        cmd.Parameters.Add("@SGSTPercentage", SqlDbType.Decimal).Value = taxType.SGSTPercentage;
                        cmd.Parameters.Add("@IGSTPercentage", SqlDbType.Decimal).Value = taxType.IGSTPercentage;
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
                        throw new Exception(taxType.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        taxType.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = taxType.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = taxType.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllTaxType
        public List<TaxType> GetAllTaxType(TaxTypeAdvanceSearch taxTypeAdvanceSearch)
        {
            List<TaxType> taxTypeList = null;
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
                        cmd.CommandText = "[PSA].[GetAllTaxType]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(taxTypeAdvanceSearch.SearchTerm) ? "" : taxTypeAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = taxTypeAdvanceSearch.DataTablePaging.Start;
                        if (taxTypeAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = taxTypeAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                taxTypeList = new List<TaxType>();
                                while (sdr.Read())
                                {
                                    TaxType taxType = new TaxType();
                                    {
                                        taxType.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : taxType.Code);
                                        taxType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : taxType.Description);
                                        taxType.CGSTPercentage = (sdr["CGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["CGSTPercentage"].ToString()) : taxType.CGSTPercentage);
                                        taxType.SGSTPercentage = (sdr["SGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["SGSTPercentage"].ToString()) : taxType.SGSTPercentage);
                                        taxType.IGSTPercentage = (sdr["IGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["IGSTPercentage"].ToString()) : taxType.IGSTPercentage);
                                        taxType.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : taxType.TotalCount);
                                        taxType.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : taxType.FilteredCount);
                                    }
                                    taxTypeList.Add(taxType);
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
            return taxTypeList;
        }
        #endregion

        #region GetTaxType
        public TaxType GetTaxType(int code)
        {
            TaxType taxType = null;
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
                        cmd.CommandText = "[PSA].[GetTaxType]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    taxType = new TaxType();
                                    taxType.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : taxType.Code);
                                    taxType.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : taxType.Description);
                                    taxType.CGSTPercentage= (sdr["CGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["CGSTPercentage"].ToString()) : taxType.CGSTPercentage);
                                    taxType.SGSTPercentage = (sdr["SGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["SGSTPercentage"].ToString()) : taxType.SGSTPercentage);
                                    taxType.IGSTPercentage = (sdr["IGSTPercentage"].ToString() != "" ? decimal.Parse(sdr["IGSTPercentage"].ToString()) : taxType.IGSTPercentage);
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
            return taxType;
        }
        #endregion

        #region CheckTaxTypeNameExist
        public bool CheckTaxTypeNameExist(TaxType taxType)
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
                        cmd.CommandText = "[PSA].[CheckTaxTypeNameExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = taxType.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar,-1).Value = taxType.Description;
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

        #region DeleteTaxType
        public object DeleteTaxType(int code)
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
                        cmd.CommandText = "[PSA].[DeleteTaxType]";
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

        #region GetTaxTypeForSelectList
        public List<TaxType> GetTaxTypeForSelectList()
        {
            List<TaxType> taxTypeList = null;
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
                        cmd.CommandText = "[PSA].[GetSelectListForTaxType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                taxTypeList = new List<TaxType>();
                                while (sdr.Read())
                                {
                                    TaxType taxType = new TaxType();
                                    {
                                        taxType.ValueText = (sdr["Text"].ToString() != "" ? (sdr["Text"].ToString()) : taxType.ValueText);
                                        taxType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : taxType.Description);
                                    }
                                    taxTypeList.Add(taxType);
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
            return taxTypeList;
        }
        #endregion
    }
}

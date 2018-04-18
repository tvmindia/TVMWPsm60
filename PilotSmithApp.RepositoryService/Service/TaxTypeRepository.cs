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
                                        taxType.Text = (sdr["Text"].ToString() != "" ? (sdr["Text"].ToString()) : taxType.Text);
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

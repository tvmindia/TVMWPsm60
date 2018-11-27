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
    public class CurrencyRepository:ICurrencyRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        public CurrencyRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetCurrencyForSelectList
        public List<Currency> GetCurrencyForSelectList()
        {
            List<Currency> currencyList = null;
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
                        cmd.CommandText = "[PSA].[GetCurrencyForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            currencyList = new List<Currency>();
                            while (sdr.Read())
                            {
                                Currency currency = new Currency();
                                {
                                    currency.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : currency.Code);
                                    currency.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : currency.Description);

                                }
                                currencyList.Add(currency);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return currencyList;
        }
        #endregion GetCurrencyForSelectList
       
    }
}

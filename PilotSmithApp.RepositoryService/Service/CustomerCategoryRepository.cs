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

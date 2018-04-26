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
    public class PaymentTermRepository:IPaymentTermRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public PaymentTermRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<PaymentTerm> GetAllPayTerm()
        {
            List<PaymentTerm> payTermList = null;
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
                        cmd.CommandText = "[PSA].[GetAllPaymentTerm]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                payTermList = new List<PaymentTerm>();
                                while (sdr.Read())
                                {
                                    PaymentTerm payTerm = new PaymentTerm();
                                    {
                                        payTerm.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : payTerm.Code);
                                        payTerm.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : payTerm.Description);
                                        payTerm.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? int.Parse(sdr["NoOfDays"].ToString()) : payTerm.NoOfDays);

                                    }
                                    payTermList.Add(payTerm);
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

            return payTermList;
        }
        public List<PaymentTerm> GetPaymentTermForSelectList()
        {
            List<PaymentTerm> payTermList = null;
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
                        cmd.CommandText = "[PSA].[GetPaymentTermForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                payTermList = new List<PaymentTerm>();
                                while (sdr.Read())
                                {
                                    PaymentTerm payTerm = new PaymentTerm();
                                    {
                                        payTerm.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : payTerm.Code);
                                        payTerm.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : payTerm.Description);
                                        payTerm.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? int.Parse(sdr["NoOfDays"].ToString()) : payTerm.NoOfDays);

                                    }
                                    payTermList.Add(payTerm);
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

            return payTermList;
        }
    }
}

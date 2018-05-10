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
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public PaymentTermRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdatePaymentTerm
        public object InsertUpdatePaymentTerm(PaymentTerm paymentTerm)
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
                        cmd.CommandText = "[PSA].[InsertUpdatePaymentTerm]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = paymentTerm.IsUpdate;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar,10).Value = paymentTerm.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = paymentTerm.Description;
                        cmd.Parameters.Add("@NoOfDays", SqlDbType.Int).Value = paymentTerm.NoOfDays;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 10);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(paymentTerm.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        paymentTerm.Code = outputCode.Value.ToString();
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = paymentTerm.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = paymentTerm.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        public List<PaymentTerm> GetAllPayTerm(PaymentTermAdvanceSearch paymentTermAdvanceSearch)
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
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(paymentTermAdvanceSearch.SearchTerm) ? "" : paymentTermAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = paymentTermAdvanceSearch.DataTablePaging.Start;
                        if (paymentTermAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = paymentTermAdvanceSearch.DataTablePaging.Length;
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
                                        payTerm.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : payTerm.TotalCount);
                                        payTerm.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : payTerm.FilteredCount);

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

        #region GetPaymentTerm
        public PaymentTerm GetPaymentTerm(string code)
        {
            PaymentTerm paymentTerm = null;
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
                        cmd.CommandText = "[PSA].[GetPaymentTerm]";
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    paymentTerm = new PaymentTerm();
                                    paymentTerm.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : paymentTerm.Code);
                                    paymentTerm.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : paymentTerm.Description);
                                    paymentTerm.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? int.Parse(sdr["NoOfDays"].ToString()) : paymentTerm.NoOfDays);
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
            return paymentTerm;
        }
        #endregion

        #region CheckPaymentTermNameExist
        public bool CheckPaymentTermNameExist(PaymentTerm paymentTerm)
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
                        cmd.CommandText = "[PSA].[CheckPaymentTermCodeExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar,10).Value = paymentTerm.Code;
                        //cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = paymentTerm.Description;
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

        #region DeletePaymentTerm
        public object DeletePaymentTerm(string code)
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
                        cmd.CommandText = "[PSA].[DeletePaymentTerm]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
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

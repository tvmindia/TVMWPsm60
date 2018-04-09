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
    public class StateRepository:IStateRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
       // public ConnectionState Connectionstate { get; private set; }

        public StateRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateState
        public object InsertUpdateState(State state)
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
                        cmd.CommandText = "[PSA].[InsertUpdateState]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = state.IsUpdate;
                        if (state.Code == 0)

                            cmd.Parameters.AddWithValue("@Code", DBNull.Value);

                        else
                            cmd.Parameters.Add("@Code", SqlDbType.Int).Value = state.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = state.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = state.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = state.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = state.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = state.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 5);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(state.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        state.Code = int.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = state.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = state.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetAllState
        public List<State> GetAllState(StateAdvanceSearch stateAdvanceSearch)
        {
            List<State> stateList = null;
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
                        cmd.CommandText = "[PSA].[GetAllState]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(stateAdvanceSearch.SearchTerm) ? "" : stateAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = stateAdvanceSearch.DataTablePaging.Start;
                        if (stateAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = stateAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stateList = new List<State>();
                                while (sdr.Read())
                                {
                                    State state = new State();
                                    {
                                        state.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : state.Code);
                                        state.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : state.Description);
                                        state.PSASysCommon = new PSASysCommon();
                                        state.PSASysCommon.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : state.PSASysCommon.CreatedDateString);
                                        state.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : state.TotalCount);
                                        state.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : state.FilteredCount);
                                    }
                                    stateList.Add(state);
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
            return stateList;
        }
        #endregion

        #region GetState
        public State GetState(int code)
        {
            State state = null;
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
                        cmd.CommandText = "[PSA].[GetState]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    state = new State();
                                    state.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : state.Code);
                                    state.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : state.Description);
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
            return state;
        }
        #endregion

        #region CheckStateNameExist
        public bool CheckStateNameExist(State state)
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
                        cmd.CommandText = "[PSA].[CheckStateNameExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.Int).Value = state.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = state.Description;
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
        #endregion CheckStateNameExist

        #region DeleteState
        public object DeleteState(int code)
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
                        cmd.CommandText = "[PSA].[DeleteState]";
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

        #region GetStateForSelectList
        public List<State> GetStateForSelectList()
        {
            List<State> stateList = null;
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
                        cmd.CommandText = "[PSA].[GetStateForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stateList = new List<State>();
                                while (sdr.Read())
                                {
                                    State state = new State();
                                    {
                                        state.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : state.Code);
                                        state.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : state.Description);
                                    }
                                    stateList.Add(state);
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
            return stateList;
        }
        #endregion
    }
}

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
    public class UserInBranchRepository : IUserInBranchRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public UserInBranchRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllUserInBranchByUserId
        /// <summary>
        /// To Get List of All Approver
        /// </summary>
        /// <param name="approverAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<UserInBranch> GetAllUserInBranchByUserId(Guid userId)
        {
            List<UserInBranch> userInBranchList = null;
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
                        cmd.CommandText = "[PSA].[GetAllUserInBranchByUserId]";
                        cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                userInBranchList = new List<UserInBranch>();
                                while (sdr.Read())
                                {
                                    UserInBranch userInBranch = new UserInBranch();
                                    {
                                        userInBranch.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : userInBranch.ID);
                                        userInBranch.UserID = (sdr["UserID"].ToString() != "" ? Guid.Parse(sdr["UserID"].ToString()) : userInBranch.UserID);
                                        userInBranch.PSAUser = new PSAUser();
                                        userInBranch.PSAUser.UserName= (sdr["UserName"].ToString() != "" ? (sdr["UserName"].ToString()) : userInBranch.PSAUser.UserName);
                                        userInBranch.Branch = new Branch();
                                        userInBranch.Branch.Description = (sdr["Branch"].ToString() != "" ? (sdr["Branch"].ToString()) : userInBranch.Branch.Description);
                                        userInBranch.BranchCode = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : userInBranch.BranchCode);
                                        userInBranch.IsDefault = (sdr["IsDefault"].ToString() != "" ? bool.Parse(sdr["IsDefault"].ToString()) : userInBranch.IsDefault);
                                        userInBranch.HasAccess = (sdr["HasAccess"].ToString() != "" ? bool.Parse(sdr["HasAccess"].ToString()) : userInBranch.HasAccess);
                                    }
                                    userInBranchList.Add(userInBranch);
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
            return userInBranchList;
        }
        #endregion GetAllUserInBranchByUserId

        #region InsertUpdateUserInBranch
        public object InsertUpdateUserInBranch(UserInBranch userInBranch)
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
                        cmd.CommandText = "[PSA].[InsertUpdateUserInBranch]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userInBranch.UserID;
                        cmd.Parameters.Add("@IsDefault", SqlDbType.VarChar,5).Value = userInBranch.DefaultBranch;
                        cmd.Parameters.Add("@HasAccess", SqlDbType.VarChar, 100).Value = userInBranch.HasAccessBranch;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = userInBranch.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = userInBranch.PSASysCommon.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception( _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message =  _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message =  _appConst.InsertSuccess
            };
        }
        #endregion

    }
}

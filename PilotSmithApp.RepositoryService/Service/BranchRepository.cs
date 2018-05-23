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
    public class BranchRepository: IBranchRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public BranchRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get Branch SelectList
        public List<Branch> GetBranchForSelectList(string loginName)
        {
            List<Branch> branchList = null;
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
                        cmd.CommandText = "[PSA].[GetBranchForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@LoginName", SqlDbType.VarChar, 250).Value = loginName;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                branchList = new List<Branch>();
                                while (sdr.Read())
                                {
                                    Branch branch = new Branch();
                                    {
                                        branch.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : branch.Code);
                                        branch.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : branch.Description);
                                        branch.UserInBranch = new UserInBranch()
                                        {
                                            IsDefault = (sdr["IsDefault"].ToString() != "" ? bool.Parse(sdr["IsDefault"].ToString()) : branch.UserInBranch.IsDefault),
                                        };
                                    }
                                    branchList.Add(branch);
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
            return branchList;
        }
        #endregion Get Branch SelectList
    }
}

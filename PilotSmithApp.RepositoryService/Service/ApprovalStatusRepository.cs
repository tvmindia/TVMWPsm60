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
   public class ApprovalStatusRepository:IApprovalStatusRepository
    {

        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ApprovalStatusRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region Get Get ApprovalStatus SelectList SelectList
        public List<ApprovalStatus> GetApprovalStatusSelectList()
        {
            List<ApprovalStatus> approvalStatusList = null;
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
                        cmd.CommandText = "[PSA].[GetApprovalStatusForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                       // cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = code;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                approvalStatusList = new List<ApprovalStatus>();
                                while (sdr.Read())
                                {
                                    ApprovalStatus approvalStatus = new ApprovalStatus();
                                    {
                                        approvalStatus.Code = (sdr["ID"].ToString() != "" ? int.Parse(sdr["ID"].ToString()) : approvalStatus.Code);
                                        approvalStatus.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : approvalStatus.Description);
                                    }
                                    approvalStatusList.Add(approvalStatus);
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
            return approvalStatusList;
        }
        #endregion Get ApprovalStatus SelectList
   
    }
}

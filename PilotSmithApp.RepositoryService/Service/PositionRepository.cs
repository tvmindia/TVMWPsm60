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
    public class PositionRepository : IPositionRepository
    {

        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        public PositionRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetPositionForSelectList
        public List<Position> GetPositionSelectList()
        {
            List<Position> positionList = null;
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
                        cmd.CommandText = "[PSA].[GetPositionForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                positionList = new List<Position>();
                                while (sdr.Read())
                                {
                                    Position position = new Position();
                                    {
                                        position.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : position.Code);
                                        position.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : position.Description);
                                    }
                                    positionList.Add(position);
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
            return positionList;
        }
        #endregion

    }
}

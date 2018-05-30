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
    public class DepartmentRepository : IDepartmentRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        public DepartmentRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetDepartmentForSelectList
        public List<Department> GetDepartmentSelectList()
        {
            List<Department> departmentList = null;
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
                        cmd.CommandText = "[PSA].[GetDepartmentForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                departmentList = new List<Department>();
                                while (sdr.Read())
                                {
                                    Department department = new Department();
                                    {
                                        department.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : department.Code);
                                        department.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : department.Description);
                                    }
                                    departmentList.Add(department);
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
            return departmentList;
        }
        #endregion

    }
}

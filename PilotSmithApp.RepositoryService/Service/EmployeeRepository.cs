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
    public class EmployeeRepository: IEmployeeRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConstant = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public EmployeeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region InsertUpdateEmployee
        public object InsertUpdateEmployee(Employee employee)
        {
            SqlParameter outputStatus, outputID = null;
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
                        cmd.CommandText = "[PSA].[InsertUpdateEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = employee.IsUpdate;
                        if(employee.ID==Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = employee.ID;
                        }
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar,10).Value = employee.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar,100).Value = employee.Name;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar,50).Value = employee.MobileNo;
                        cmd.Parameters.Add("@Address", SqlDbType.NVarChar,-1).Value = employee.Address;
                        cmd.Parameters.Add("@ImageURL", SqlDbType.NVarChar, -1).Value = employee.ImageURL;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.Int).Value = employee.DepartmentCode;
                        cmd.Parameters.Add("@PositionCode", SqlDbType.Int).Value = employee.PositionCode;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = employee.IsActive;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar,-1).Value = employee.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = employee.PSASysCommon.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = employee.PSASysCommon.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = employee.PSASysCommon.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = employee.PSASysCommon.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConstant.InsertFailure);
                    case "1":
                        employee.ID = Guid.Parse(outputID.Value.ToString());
                        return new
                        {
                            ID = employee.ID,
                            Status = outputStatus.Value.ToString(),
                            Message = employee.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
                        };
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = employee.ID,
                Status = outputStatus.Value.ToString(),
                Message = employee.IsUpdate ? _appConstant.UpdateSuccess : _appConstant.InsertSuccess
            };
        }
        #endregion InsertUpdateEmployee

        #region GetEmployee
        public Employee GetEmployee(Guid id)
        {
            Employee employee = new Employee();
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
                        cmd.CommandText = "[PSA].[GetEmployee]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    employee.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employee.ID);
                                    employee.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employee.Code);
                                    employee.Name= (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employee.Name);
                                    employee.MobileNo= (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : employee.MobileNo);
                                    employee.Address= (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : employee.Address);
                                    employee.ImageURL= (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : employee.ImageURL);
                                    employee.DepartmentCode= (sdr["DepartmentCode"].ToString() != "" ? int.Parse(sdr["DepartmentCode"].ToString()): employee.DepartmentCode);
                                    employee.PositionCode= (sdr["PositionCode"].ToString() != "" ? int.Parse(sdr["PositionCode"].ToString()) : employee.PositionCode);
                                    employee.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employee.GeneralNotes);
                                }
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return employee;
        }
        #endregion GetEmployee

        #region GetAllEmployee
        public List<Employee> GetAllEmployee(EmployeeAdvanceSearch employeeAdvanceSearch)
        {
            List<Employee> employeeList = null;
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
                        cmd.CommandText = "[PSA].[GetAllEmployee]";
                        if (string.IsNullOrEmpty(employeeAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = employeeAdvanceSearch.SearchTerm;
                        }
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = employeeAdvanceSearch.DataTablePaging.Start;
                        if (employeeAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = employeeAdvanceSearch.DataTablePaging.Length;
                       
                        //cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = employeeAdvanceSearch.FromDate;
                        //cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = employeeAdvanceSearch.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeList = new List<Employee>();
                                while(sdr.Read())
                                {
                                    Employee employee = new Employee();
                                    {
                                        employee.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employee.ID);
                                        employee.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employee.Code);
                                        employee.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employee.Name);
                                        employee.MobileNo = (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : employee.MobileNo);
                                        employee.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : employee.Address);
                                        employee.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : employee.ImageURL);
                                        employee.DepartmentCode = (sdr["DepartmentCode"].ToString() != "" ? int.Parse(sdr["DepartmentCode"].ToString()) : employee.DepartmentCode);
                                        employee.PositionCode = (sdr["PositionCode"].ToString() != "" ? int.Parse(sdr["PositionCode"].ToString()) : employee.PositionCode);
                                        employee.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employee.GeneralNotes);
                                        employee.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : employee.FilteredCount);
                                        employee.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : employee.FilteredCount);
                                    }
                                    employeeList.Add(employee);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return employeeList;
        }
        #endregion GetAllEmployee

        #region DeleteEmployee
        public object DeleteEmployee(Guid id)
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
                        cmd.CommandText = "[PSA].[DeleteEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(_appConstant.DeleteFailure);

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
                Message = _appConstant.DeleteSuccess
            };
        }
        #endregion DeleteEmployee

        #region Get Employee Dropdown
        public List<Employee> GetEmployeeSelectList()
        {
            List<Employee> employeeList = null;
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
                        cmd.CommandText = "[PSA].[GetEmployeeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeList = new List<Employee>();
                                while (sdr.Read())
                                {
                                    Employee employee = new Employee();
                                    {
                                        employee.ID= (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employee.ID);
                                        employee.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employee.Code);
                                        employee.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employee.Name);
                                    }
                                    employeeList.Add(employee);
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
            return employeeList;
        }
        #endregion Get Employee Dropdown
    }
}

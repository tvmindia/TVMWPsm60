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
    public class EnquiryGradeRepository: IEnquiryGradeRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public EnquiryGradeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region Get EnquiryGrade SelectList
        public List<EnquiryGrade> GetEnquiryGradeSelectList()
        {
            List<EnquiryGrade> enquiryGradeList = null;
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
                        cmd.CommandText = "[PSA].[GetEnquiryGradeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                enquiryGradeList = new List<EnquiryGrade>();
                                while (sdr.Read())
                                {
                                    EnquiryGrade enquiryGrade = new EnquiryGrade();
                                    {
                                        enquiryGrade.Code = (sdr["Code"].ToString() != "" ? int.Parse(sdr["Code"].ToString()) : enquiryGrade.Code);
                                        enquiryGrade.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : enquiryGrade.Description);
                                    }
                                    enquiryGradeList.Add(enquiryGrade);
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
            return enquiryGradeList;
        }
        #endregion Get EnquiryGrade SelectList
    }
}

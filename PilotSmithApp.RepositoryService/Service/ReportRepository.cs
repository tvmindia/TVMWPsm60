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
   public class ReportRepository:IReportRepository
    {
        Settings _settings = new Settings();
        AppConst _appConstant = new AppConst();      
        private IDatabaseFactory _databaseFactory;
        public ReportRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllReport
        /// <summary>
        /// To Get All Reports
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<PSASysReport> GetAllReport(string searchTerm)
        {
            List<PSASysReport> PSASysReportList = null;
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
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(searchTerm) ? "" : searchTerm;
                        cmd.CommandText = "[PSA].[GetAllSysReports]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                PSASysReportList = new List<PSASysReport>();
                                while (sdr.Read())
                                {
                                    PSASysReport psaSysReport = new PSASysReport();
                                    {
                                        psaSysReport.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : psaSysReport.ID);
                                        psaSysReport.ReportName = (sdr["ReportName"].ToString() != "" ? (sdr["ReportName"].ToString()) : psaSysReport.ReportName);
                                        psaSysReport.ReportDescription = (sdr["ReportDescription"].ToString() != "" ? (sdr["ReportDescription"].ToString()) : psaSysReport.ReportDescription);
                                        psaSysReport.Controller = (sdr["Controller"].ToString() != "" ? sdr["Controller"].ToString() : psaSysReport.Controller);
                                        psaSysReport.Action = (sdr["Action"].ToString() != "" ? sdr["Action"].ToString() : psaSysReport.Action);
                                        psaSysReport.SPName = (sdr["SPName"].ToString() != "" ? sdr["SPName"].ToString() : psaSysReport.SPName);
                                        psaSysReport.SQL = (sdr["SQL"].ToString() != "" ? sdr["SQL"].ToString() : psaSysReport.SQL);
                                        psaSysReport.ReportOrder = (sdr["ReportOrder"].ToString() != "" ? int.Parse(sdr["ReportOrder"].ToString()) : psaSysReport.ReportOrder);
                                        psaSysReport.ReportGroup = (sdr["ReportGroup"].ToString() != "" ? sdr["ReportGroup"].ToString() : psaSysReport.ReportGroup);
                                        psaSysReport.GroupOrder = (sdr["GroupOrder"].ToString() != "" ? int.Parse(sdr["GroupOrder"].ToString()) : psaSysReport.GroupOrder);
                                    }
                                    PSASysReportList.Add(psaSysReport);
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
            return PSASysReportList;
        }
        #endregion GetAllReport


        //#region GetPendingSaleOrderProductionReport
        //public List<PendingSaleOrderProductionReport> GetPendingSaleOrderProductionReport(PendingSaleOrderProductionReport pendingSaleOrderProductionReport)
        //{

        //    List<PendingSaleOrderProductionReport> pendingSaleOrderProductionReportList = null;
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[PSA].[GetSaleOrderPendingBasedOnProductionReport]";
        //                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(pendingSaleOrderProductionReport.SearchTerm) ? "" : pendingSaleOrderProductionReport.SearchTerm;
        //                //cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = pendingSaleOrderProductionReport.DataTablePaging.Start;
        //                //cmd.Parameters.Add("@Length", SqlDbType.Int).Value = pendingSaleOrderProductionReport.DataTablePaging.Length;
        //                //cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = pendingSaleOrderProductionReport.DataTablePaging.OrderDir;
        //                //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = pendingSaleOrderProductionReport.DataTablePaging.OrderColumn;
        //                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = pendingSaleOrderProductionReport.AdvFromDate;
        //                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = pendingSaleOrderProductionReport.AdvToDate;
        //                if (pendingSaleOrderProductionReport.AdvCustomerID != Guid.Empty)
        //                    cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = pendingSaleOrderProductionReport.AdvCustomerID;
        //                if (pendingSaleOrderProductionReport.AdvProductID != Guid.Empty)
        //                    cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = pendingSaleOrderProductionReport.AdvProductID;                       
        //                cmd.Parameters.Add("@SaleOrderNo", SqlDbType.NVarChar,50).Value = pendingSaleOrderProductionReport.AdvSaleOrderNo;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    if ((sdr != null) && (sdr.HasRows))
        //                    {
        //                        pendingSaleOrderProductionReportList = new List<PendingSaleOrderProductionReport>();
        //                        while (sdr.Read())
        //                        {
        //                            PendingSaleOrderProductionReport pendingSaleOrderReportObj = new PendingSaleOrderProductionReport();
        //                            {                                        
        //                                pendingSaleOrderReportObj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : pendingSaleOrderReportObj.SaleOrderNo);
        //                                pendingSaleOrderReportObj.SaleOrderDate = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()) : pendingSaleOrderReportObj.SaleOrderDate);
        //                                pendingSaleOrderReportObj.SaleOrderDateFormatted = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()).ToString(_settings.DateFormat) : pendingSaleOrderReportObj.SaleOrderDateFormatted);
        //                                pendingSaleOrderReportObj.Customer = new Customer();                                       
        //                                pendingSaleOrderReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : pendingSaleOrderReportObj.Customer.CompanyName);
        //                                pendingSaleOrderReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : pendingSaleOrderReportObj.Customer.ContactPerson);
        //                                pendingSaleOrderReportObj.Product = new Product();
        //                                pendingSaleOrderReportObj.Product.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : pendingSaleOrderReportObj.Product.Name);
        //                                pendingSaleOrderReportObj.SaleOrderQty= (sdr["OrderedQty"].ToString() != "" ? decimal.Parse(sdr["OrderedQty"].ToString()) : pendingSaleOrderReportObj.SaleOrderQty);
        //                                pendingSaleOrderReportObj.OrderQty = (sdr["ProductionOrderQty"].ToString() != "" ? decimal.Parse(sdr["ProductionOrderQty"].ToString()) : pendingSaleOrderReportObj.OrderQty);
        //                                pendingSaleOrderReportObj.ProducedQty = (sdr["ProducedQty"].ToString() != "" ? decimal.Parse(sdr["ProducedQty"].ToString()) : pendingSaleOrderReportObj.ProducedQty);
        //                                pendingSaleOrderReportObj.PendingQty = (sdr["PendingQty"].ToString() != "" ? decimal.Parse(sdr["PendingQty"].ToString()) : pendingSaleOrderReportObj.PendingQty);                                    

        //                            }
        //                            pendingSaleOrderProductionReportList.Add(pendingSaleOrderReportObj);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return pendingSaleOrderProductionReportList;
        //}
        //#endregion GetPendingSaleOrderProductionReport


        #region GetEnquiryReport
        /// <summary>
        /// To Get Enquiry Report
        /// </summary>
        /// <param name="enquiryReport"></param>
        /// <returns></returns>
        public List<EnquiryReport> GetEnquiryReport(EnquiryReport enquiryReport)
        {

            List<EnquiryReport> enquiryReportList = null;
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
                        cmd.CommandText = "[PSA].[EnquiryReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(enquiryReport.SearchTerm) ? "" : enquiryReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = enquiryReport.DataTablePaging.Start;
                        if (enquiryReport.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = enquiryReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = enquiryReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = enquiryReport.AdvToDate;
                        if (enquiryReport.AdvCustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = enquiryReport.AdvCustomerID;
                        if (enquiryReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = enquiryReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = enquiryReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@ReferencePersonCode", SqlDbType.NVarChar, 50).Value = enquiryReport.AdvReferencePersonCode;
                        cmd.Parameters.Add("@ReferenceTypeCode", SqlDbType.NVarChar, 50).Value = enquiryReport.AdvReferenceTypeCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.NVarChar, 50).Value = enquiryReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.NVarChar, 50).Value = enquiryReport.AdvBranchCode;                        
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = enquiryReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = enquiryReport.AdvAmountTo;
                        cmd.Parameters.Add("@GradeCode", SqlDbType.Int).Value = enquiryReport.AdvEnquiryGradeCode;
                        if (enquiryReport.AdvAttendedByID != Guid.Empty)
                            cmd.Parameters.Add("@AttendedByID", SqlDbType.UniqueIdentifier).Value = enquiryReport.AdvAttendedByID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                enquiryReportList = new List<EnquiryReport>();
                                while (sdr.Read())
                                {
                                    EnquiryReport enquiryReportObj = new EnquiryReport();
                                    {
                                        enquiryReportObj.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? (sdr["EnquiryNo"].ToString()) : enquiryReportObj.EnquiryNo);
                                        enquiryReportObj.EnquiryDate = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()) : enquiryReportObj.EnquiryDate);
                                        enquiryReportObj.EnquiryDateFormatted = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()).ToString(_settings.DateFormat) : enquiryReportObj.EnquiryDateFormatted);
                                        enquiryReportObj.Customer = new Customer();
                                        enquiryReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : enquiryReportObj.Customer.CompanyName);
                                        enquiryReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : enquiryReportObj.Customer.ContactPerson);                                       
                                        enquiryReportObj.RequirementSpec = (sdr["RequirementSpec"].ToString() != "" ? sdr["RequirementSpec"].ToString() : enquiryReportObj.RequirementSpec);
                                        enquiryReportObj.Area = new Area();
                                        enquiryReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : enquiryReportObj.Area.Description);
                                        enquiryReportObj.Branch = new Branch();
                                        enquiryReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : enquiryReportObj.Branch.Description);
                                        enquiryReportObj.ReferencePerson = new ReferencePerson();
                                        enquiryReportObj.ReferencePerson.Name = (sdr["ReferredBy"].ToString() != "" ? sdr["ReferredBy"].ToString() : enquiryReportObj.ReferencePerson.Name);                                       
                                        enquiryReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : enquiryReportObj.Amount);
                                        enquiryReportObj.PSAUser = new PSAUser();
                                        enquiryReportObj.PSAUser.LoginName= (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : enquiryReportObj.PSAUser.LoginName);
                                        enquiryReportObj.DocumentStatus = new DocumentStatus();
                                        enquiryReportObj.DocumentStatus.Description = (sdr["DocumentStatus"].ToString() != "" ? sdr["DocumentStatus"].ToString() : enquiryReportObj.DocumentStatus.Description);
                                        enquiryReportObj.EnquiryGrade = new EnquiryGrade();
                                        enquiryReportObj.EnquiryGrade.Description = (sdr["GradeName"].ToString() != "" ? sdr["GradeName"].ToString() : enquiryReportObj.EnquiryGrade.Description);
                                        enquiryReportObj.TotalCount= (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : enquiryReportObj.TotalCount);
                                        enquiryReportObj.FilteredCount= (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : enquiryReportObj.FilteredCount);
                                        enquiryReportObj.Employee = new Employee();
                                        enquiryReportObj.Employee.Name= (sdr["EmployeeName"].ToString() != "" ? sdr["EmployeeName"].ToString() : enquiryReportObj.Employee.Name);
                                    }
                                    enquiryReportList.Add(enquiryReportObj);
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

            return enquiryReportList;
        }
        #endregion GetEnquiryReport
    }
}

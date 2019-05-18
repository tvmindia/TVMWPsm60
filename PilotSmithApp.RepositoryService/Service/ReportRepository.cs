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
    public class ReportRepository : IReportRepository
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
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = enquiryReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = enquiryReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = enquiryReport.AdvToDate;                       
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar,100).Value = enquiryReport.AdvCustomer;
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
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = enquiryReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = enquiryReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = enquiryReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = enquiryReport.AdvCustomerCategoryCode;
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
                                        enquiryReportObj.EnquiryNo = (sdr["EnqNo"].ToString() != "" ? (sdr["EnqNo"].ToString()) : enquiryReportObj.EnquiryNo);
                                        enquiryReportObj.EnqNo = (sdr["EnquiryNo"].ToString() != "" ? (sdr["EnquiryNo"].ToString()) : enquiryReportObj.EnqNo);

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
                                        enquiryReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : enquiryReportObj.PSAUser.LoginName);
                                        enquiryReportObj.DocumentStatus = new DocumentStatus();
                                        enquiryReportObj.DocumentStatus.Description = (sdr["DocumentStatus"].ToString() != "" ? sdr["DocumentStatus"].ToString() : enquiryReportObj.DocumentStatus.Description);
                                        enquiryReportObj.EnquiryGrade = new EnquiryGrade();
                                        enquiryReportObj.EnquiryGrade.Description = (sdr["GradeName"].ToString() != "" ? sdr["GradeName"].ToString() : enquiryReportObj.EnquiryGrade.Description);
                                        enquiryReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : enquiryReportObj.TotalCount);
                                        enquiryReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : enquiryReportObj.FilteredCount);
                                        enquiryReportObj.Employee = new Employee();
                                        enquiryReportObj.Employee.Name = (sdr["EmployeeName"].ToString() != "" ? sdr["EmployeeName"].ToString() : enquiryReportObj.Employee.Name);
                                        enquiryReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : enquiryReportObj.TotalAmount);
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


        #region GetEnquiryFollowupReport
        /// <summary>
        /// To  Get Enquiry FollowupReport
        /// </summary>
        /// <param name="enquiryFollowupReport"></param>
        /// <returns></returns>
        public List<EnquiryFollowupReport> GetEnquiryFollowupReport(EnquiryFollowupReport enquiryFollowupReport)
        {
            List<EnquiryFollowupReport> enquiryFollwupReportList = null;
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
                        cmd.CommandText = "[PSA].[GetEnqueryFollowUpReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(enquiryFollowupReport.SearchTerm) ? "" : enquiryFollowupReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = enquiryFollowupReport.DataTablePaging.Start;
                        if (enquiryFollowupReport.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = enquiryFollowupReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = enquiryFollowupReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = enquiryFollowupReport.AdvToDate;                       
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar,100).Value = enquiryFollowupReport.AdvCustomer;
                        if (enquiryFollowupReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = enquiryFollowupReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@PriorityCode", SqlDbType.Int).Value = enquiryFollowupReport.AdvFollowupPriority;
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = enquiryFollowupReport.AdvStatus;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                enquiryFollwupReportList = new List<EnquiryFollowupReport>();
                                while (sdr.Read())
                                {
                                    EnquiryFollowupReport enquiryFollowupReportObj = new EnquiryFollowupReport();
                                    {

                                        enquiryFollowupReportObj.EnquiryDate = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()) : enquiryFollowupReportObj.EnquiryDate);
                                        enquiryFollowupReportObj.EnquiryNo = (sdr["EnqNo"].ToString() != "" ? (sdr["EnqNo"].ToString()) : enquiryFollowupReportObj.EnquiryNo);
                                        enquiryFollowupReportObj.EnqNo = (sdr["EnquiryNo"].ToString() != "" ? (sdr["EnquiryNo"].ToString()) : enquiryFollowupReportObj.EnqNo);

                                        enquiryFollowupReportObj.EnquiryDateFormatted = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()).ToString(_settings.DateFormat) : enquiryFollowupReportObj.EnquiryDateFormatted);
                                        
                                        enquiryFollowupReportObj.Priority = (sdr["Priority"].ToString() != "" ? sdr["Priority"].ToString() : enquiryFollowupReportObj.Priority);

                                        enquiryFollowupReportObj.FollowupDate = (sdr["FollowupDate"].ToString() != "" ? DateTime.Parse(sdr["FollowupDate"].ToString()) : enquiryFollowupReportObj.FollowupDate);
                                        enquiryFollowupReportObj.FollowupDateFormatted = (sdr["FollowupDate"].ToString() != "" ? DateTime.Parse(sdr["FollowupDate"].ToString()).ToString(_settings.DateFormat) : enquiryFollowupReportObj.FollowupDateFormatted);
                                        enquiryFollowupReportObj.FollowupTimeFormatted = (sdr["FollowupTime"].ToString() != "" ? DateTime.Parse(sdr["FollowUpTime"].ToString()).ToString("h:mm tt") : enquiryFollowupReportObj.FollowupTimeFormatted);
                                        enquiryFollowupReportObj.PriorityCode = (sdr["PriorityCode"].ToString() != "" ? int.Parse(sdr["PriorityCode"].ToString()) : enquiryFollowupReportObj.PriorityCode);
                                        enquiryFollowupReportObj.FollowupRemarks = (sdr["Subject"].ToString() != "" ? sdr["Subject"].ToString() : enquiryFollowupReportObj.FollowupRemarks);
                                        enquiryFollowupReportObj.ContactName = (sdr["ContactName"].ToString() != "" ? sdr["ContactName"].ToString() : enquiryFollowupReportObj.ContactName);
                                        enquiryFollowupReportObj.ContactNo = (sdr["ContactNo"].ToString() != "" ? (sdr["ContactNo"].ToString()) : enquiryFollowupReportObj.ContactNo);
                                        enquiryFollowupReportObj.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : enquiryFollowupReportObj.Status);
                                        enquiryFollowupReportObj.Customer = new Customer();
                                        enquiryFollowupReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : enquiryFollowupReportObj.Customer.CompanyName);
                                        enquiryFollowupReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : enquiryFollowupReportObj.Customer.ContactPerson);
                                        enquiryFollowupReportObj.DocumentOwnerName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : enquiryFollowupReportObj.DocumentOwnerName);
                                        enquiryFollowupReportObj.TotalCount=(sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : enquiryFollowupReportObj.TotalCount);
                                        enquiryFollowupReportObj.FilteredCount=(sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : enquiryFollowupReportObj.FilteredCount);

                                    }
                                    enquiryFollwupReportList.Add(enquiryFollowupReportObj);
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

            return enquiryFollwupReportList;
        }
        #endregion GetEnquiryFollowupReport


        #region GetEstimateReport
        /// <summary>
        /// To Get Estimate Report
        /// </summary>
        /// <param name="estimateReport"></param>
        /// <returns></returns>
        public List<EstimateReport> GetEstimateReport(EstimateReport estimateReport)
        {

            List<EstimateReport> estimateReportList = null;
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
                        cmd.CommandText = "[PSA].[GetEstimateReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = estimateReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = estimateReport.DataTablePaging.Start;
                        if (estimateReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = estimateReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = estimateReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = estimateReport.AdvToDate;                       
                            cmd.Parameters.Add("@Customer", SqlDbType.NVarChar,100).Value = estimateReport.AdvCustomer;
                        if (estimateReport.AdvPreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = estimateReport.AdvPreparedBy;
                        if (estimateReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = estimateReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = estimateReport.AdvDocumentStatusCode;                      
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = estimateReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = estimateReport.AdvBranchCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = estimateReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = estimateReport.AdvAmountTo;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = estimateReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = estimateReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = estimateReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = estimateReport.AdvCustomerCategoryCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                estimateReportList = new List<EstimateReport>();
                                while (sdr.Read())
                                {
                                    EstimateReport estimateReportObj = new EstimateReport();
                                    {
                                        estimateReportObj.EstimateNo = (sdr["EstNo"].ToString() != "" ? (sdr["EstNo"].ToString()) : estimateReportObj.EstimateNo);
                                        estimateReportObj.EstNo = (sdr["EstimateNo"].ToString() != "" ? (sdr["EstimateNo"].ToString()) : estimateReportObj.EstNo);

                                        estimateReportObj.EstimateDate = (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()) : estimateReportObj.EstimateDate);
                                      
                                        estimateReportObj.EstimateDateFormatted= (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()).ToString(_settings.DateFormat) : estimateReportObj.EstimateDateFormatted);
                                        estimateReportObj.Customer = new Customer();
                                        estimateReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : estimateReportObj.Customer.CompanyName);
                                        estimateReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : estimateReportObj.Customer.ContactPerson);
                                      
                                        estimateReportObj.Area = new Area();
                                        estimateReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : estimateReportObj.Area.Description);
                                        estimateReportObj.Branch = new Branch();
                                        estimateReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : estimateReportObj.Branch.Description);
                                      
                                        estimateReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : estimateReportObj.Amount);
                                        estimateReportObj.PSAUser = new PSAUser();
                                        estimateReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : estimateReportObj.PSAUser.LoginName);
                                        estimateReportObj.DocumentStatus = new DocumentStatus();
                                        estimateReportObj.DocumentStatus.Description = (sdr["DocumentStatusName"].ToString() != "" ? sdr["DocumentStatusName"].ToString() : estimateReportObj.DocumentStatus.Description);                                       
                                        estimateReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : estimateReportObj.TotalCount);
                                        estimateReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : estimateReportObj.FilteredCount);
                                        estimateReportObj.Notes= (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : estimateReportObj.Notes);
                                        estimateReportObj.PreparedBy = (sdr["PreparedByName"].ToString() != "" ? sdr["PreparedByName"].ToString() : estimateReportObj.PreparedBy);
                                        estimateReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : estimateReportObj.TotalAmount);
                                    }
                                    estimateReportList.Add(estimateReportObj);
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

            return estimateReportList;
        }
        #endregion GetEstimateReport


        #region GetQuotationReport
        /// <summary>
        /// To  Get Quotation Report
        /// </summary>
        /// <param name="quotationReport"></param>
        /// <returns></returns>
        public List<QuotationReport> GetQuotationReport(QuotationReport quotationReport)
        {

            List<QuotationReport> quotationReportList = null;
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
                        cmd.CommandText = "[PSA].[GetQuotationReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(quotationReport.SearchTerm) ? "" : quotationReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = quotationReport.DataTablePaging.Start;
                        if (quotationReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = quotationReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = quotationReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = quotationReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar,100).Value = quotationReport.AdvCustomer;
                        if (quotationReport.AdvPreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = quotationReport.AdvPreparedBy;                       
                            cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = quotationReport.AdvReferencePersonCode;
                        if (quotationReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = quotationReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = quotationReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = quotationReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = quotationReport.AdvBranchCode;                       
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = quotationReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = quotationReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = quotationReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = quotationReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = quotationReport.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = quotationReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = quotationReport.AdvAmountTo;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                quotationReportList = new List<QuotationReport>();
                                while (sdr.Read())
                                {
                                    QuotationReport quotationReportObj = new QuotationReport();
                                    {
                                        quotationReportObj.QuoteNo = (sdr["QuotationNo"].ToString() != "" ? (sdr["QuotationNo"].ToString()) : quotationReportObj.QuoteNo);
                                        quotationReportObj.QuotationNo = (sdr["QuoteNo"].ToString() != "" ? (sdr["QuoteNo"].ToString()) : quotationReportObj.QuotationNo);

                                        quotationReportObj.QuoteDate = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()) : quotationReportObj.QuoteDate);
                                        quotationReportObj.QuoteDateFormatted = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()).ToString(_settings.DateFormat) : quotationReportObj.QuoteDateFormatted);
                                        quotationReportObj.Customer = new Customer();
                                        quotationReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : quotationReportObj.Customer.CompanyName);
                                        quotationReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : quotationReportObj.Customer.ContactPerson);

                                        quotationReportObj.Area = new Area();
                                        quotationReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : quotationReportObj.Area.Description);
                                        quotationReportObj.Branch = new Branch();
                                        quotationReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : quotationReportObj.Branch.Description);

                                        quotationReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : quotationReportObj.Amount);
                                        quotationReportObj.PSAUser = new PSAUser();
                                        quotationReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : quotationReportObj.PSAUser.LoginName);
                                        quotationReportObj.DocumentStatus = new DocumentStatus();
                                        quotationReportObj.DocumentStatus.Description = (sdr["DocumentStatusName"].ToString() != "" ? sdr["DocumentStatusName"].ToString() : quotationReportObj.DocumentStatus.Description);
                                        quotationReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : quotationReportObj.TotalCount);
                                        quotationReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : quotationReportObj.FilteredCount);
                                        quotationReportObj.Notes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : quotationReportObj.Notes);
                                        quotationReportObj.PreparedBy = (sdr["PreparedByName"].ToString() != "" ? sdr["PreparedByName"].ToString() : quotationReportObj.PreparedBy);
                                        quotationReportObj.ReferencePerson = new ReferencePerson();
                                        quotationReportObj.ReferencePerson.Name = (sdr["ReferredByName"].ToString() != "" ? sdr["ReferredByName"].ToString() : quotationReportObj.ReferencePerson.Name);
                                        quotationReportObj.ApprovalStatus = new ApprovalStatus();
                                        quotationReportObj.ApprovalStatus.Description = (sdr["LatestApprovalStatusName"].ToString() != "" ? sdr["LatestApprovalStatusName"].ToString() : quotationReportObj.ApprovalStatus.Description);
                                        quotationReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : quotationReportObj.TotalAmount);
                                    }
                                    quotationReportList.Add(quotationReportObj);
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

            return quotationReportList;
        }
        #endregion GetQuotationReport

        #region GetPendingSaleOrderReport
        public List<PendingSaleOrderReport> GetPendingSaleOrderReport(PendingSaleOrderReport pendingSaleOrderReport)
        {

            List<PendingSaleOrderReport> pendingSaleOrderReportList = null;
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
                        cmd.CommandText = "[PSA].[GetPendingSaleOrderReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(pendingSaleOrderReport.SearchTerm) ? "" : pendingSaleOrderReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = pendingSaleOrderReport.DataTablePaging.Start;
                        if (pendingSaleOrderReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = pendingSaleOrderReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = pendingSaleOrderReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = pendingSaleOrderReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = pendingSaleOrderReport.AdvCustomer;
                        if (pendingSaleOrderReport.AdvPreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = pendingSaleOrderReport.AdvPreparedBy;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = pendingSaleOrderReport.AdvReferencePersonCode;
                        if (pendingSaleOrderReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = pendingSaleOrderReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = pendingSaleOrderReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = pendingSaleOrderReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = pendingSaleOrderReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = pendingSaleOrderReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = pendingSaleOrderReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = pendingSaleOrderReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = pendingSaleOrderReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = pendingSaleOrderReport.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = pendingSaleOrderReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = pendingSaleOrderReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = pendingSaleOrderReport.AdvReportType;
                        cmd.Parameters.Add("@ExpDelDateFrom", SqlDbType.DateTime).Value = pendingSaleOrderReport.AdvDelFromDate;
                        cmd.Parameters.Add("@ExpDelDateTo", SqlDbType.DateTime).Value = pendingSaleOrderReport.AdvDelToDate;
                        if (pendingSaleOrderReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = pendingSaleOrderReport.AdvProduct;
                        if (pendingSaleOrderReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = pendingSaleOrderReport.AdvProductModel;





                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                pendingSaleOrderReportList = new List<PendingSaleOrderReport>();
                                while (sdr.Read())
                                {
                                    PendingSaleOrderReport pendingSaleOrderReportObj = new PendingSaleOrderReport();
                                    {
                                        pendingSaleOrderReportObj.SaleOrderNo = (sdr["SaleOrdNo"].ToString() != "" ? (sdr["SaleOrdNo"].ToString()) : pendingSaleOrderReportObj.SaleOrderNo);
                                        pendingSaleOrderReportObj.SaleOrdNo= (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : pendingSaleOrderReportObj.SaleOrdNo);
                                        pendingSaleOrderReportObj.SaleOrderDate = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()) : pendingSaleOrderReportObj.SaleOrderDate);
                                        pendingSaleOrderReportObj.SaleOrderDateFormatted = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()).ToString(_settings.DateFormat) : pendingSaleOrderReportObj.SaleOrderDateFormatted);
                                        pendingSaleOrderReportObj.Customer = new Customer();
                                        pendingSaleOrderReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : pendingSaleOrderReportObj.Customer.CompanyName);
                                        pendingSaleOrderReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : pendingSaleOrderReportObj.Customer.ContactPerson);                                       
                                        pendingSaleOrderReportObj.Branch = new Branch();
                                        pendingSaleOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : pendingSaleOrderReportObj.Branch.Description);
                                        pendingSaleOrderReportObj.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : pendingSaleOrderReportObj.TaxableAmount);
                                        pendingSaleOrderReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : pendingSaleOrderReportObj.Amount);
                                        pendingSaleOrderReportObj.PSAUser = new PSAUser();
                                        pendingSaleOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : pendingSaleOrderReportObj.PSAUser.LoginName);                                       
                                        pendingSaleOrderReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : pendingSaleOrderReportObj.TotalCount);
                                        pendingSaleOrderReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : pendingSaleOrderReportObj.FilteredCount);
                                        pendingSaleOrderReportObj.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : pendingSaleOrderReportObj.Qty);
                                        pendingSaleOrderReportObj.PendingQty = (sdr["PendingQty"].ToString() != "" ? decimal.Parse(sdr["PendingQty"].ToString()) : pendingSaleOrderReportObj.PendingQty);
                                        pendingSaleOrderReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : pendingSaleOrderReportObj.ProductSpec);
                                        pendingSaleOrderReportObj.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : pendingSaleOrderReportObj.ProductID);
                                        pendingSaleOrderReportObj.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : pendingSaleOrderReportObj.ProductModelID);
                                        pendingSaleOrderReportObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : pendingSaleOrderReportObj.UnitCode);
                                        pendingSaleOrderReportObj.Product = new Product();
                                        pendingSaleOrderReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : pendingSaleOrderReportObj.Product.Name);
                                        pendingSaleOrderReportObj.ProductModel = new ProductModel();
                                        pendingSaleOrderReportObj.ProductModel.Name = (sdr["ProductModalName"].ToString() != "" ? sdr["ProductModalName"].ToString() : pendingSaleOrderReportObj.ProductModel.Name);
                                        pendingSaleOrderReportObj.Unit = new Unit();
                                        pendingSaleOrderReportObj.Unit.Description= (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : pendingSaleOrderReportObj.Unit.Description);
                                        pendingSaleOrderReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : pendingSaleOrderReportObj.TotalAmount);
                                        pendingSaleOrderReportObj.TotalTaxableAmount = (sdr["TotalTaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TotalTaxableAmount"].ToString()) : pendingSaleOrderReportObj.TotalTaxableAmount);

                                    }
                                    pendingSaleOrderReportList.Add(pendingSaleOrderReportObj);
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

            return pendingSaleOrderReportList;
        }
        #endregion GetPendingSaleOrderReport


        #region GetSaleOrderStandardReport
        public List<SaleOrderReport> GetSaleOrderStandardReport(SaleOrderReport saleOrderReport)
        {

            List<SaleOrderReport> saleOrderReportList = null;
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
                        cmd.CommandText = "[PSA].[GetSaleOrderStandardReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(saleOrderReport.SearchTerm) ? "" : saleOrderReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = saleOrderReport.DataTablePaging.Start;
                        if (saleOrderReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = saleOrderReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = saleOrderReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = saleOrderReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = saleOrderReport.AdvCustomer;
                        if (saleOrderReport.AdvPreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = saleOrderReport.AdvPreparedBy;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = saleOrderReport.AdvReferencePersonCode;
                        if (saleOrderReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = saleOrderReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = saleOrderReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = saleOrderReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = saleOrderReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = saleOrderReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = saleOrderReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = saleOrderReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = saleOrderReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = saleOrderReport.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = saleOrderReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = saleOrderReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = saleOrderReport.AdvReportType;
                        cmd.Parameters.Add("@ExpDelDateFrom", SqlDbType.DateTime).Value = saleOrderReport.AdvDelFromDate;
                        cmd.Parameters.Add("@ExpDelDateTo", SqlDbType.DateTime).Value = saleOrderReport.AdvDelToDate;
                        if (saleOrderReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = saleOrderReport.AdvProduct;
                        if (saleOrderReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = saleOrderReport.AdvProductModel;
                        
       
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                saleOrderReportList = new List<SaleOrderReport>();
                                while (sdr.Read())
                                {
                                    SaleOrderReport saleOrderReportObj = new SaleOrderReport();
                                    {
                                        saleOrderReportObj.SaleOrderNo = (sdr["SaleOrdNo"].ToString() != "" ? (sdr["SaleOrdNo"].ToString()) : saleOrderReportObj.SaleOrderNo);
                                        saleOrderReportObj.SaleOrdNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : saleOrderReportObj.SaleOrdNo);
                                        saleOrderReportObj.SaleOrderDate = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()) : saleOrderReportObj.SaleOrderDate);
                                        saleOrderReportObj.SaleOrderDateFormatted = (sdr["SaleOrderDate"].ToString() != "" ? DateTime.Parse(sdr["SaleOrderDate"].ToString()).ToString(_settings.DateFormat) : saleOrderReportObj.SaleOrderDateFormatted);
                                        saleOrderReportObj.Customer = new Customer();
                                        saleOrderReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : saleOrderReportObj.Customer.CompanyName);
                                        saleOrderReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : saleOrderReportObj.Customer.ContactPerson);
                                        saleOrderReportObj.Branch = new Branch();
                                        saleOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : saleOrderReportObj.Branch.Description);
                                        saleOrderReportObj.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : saleOrderReportObj.TaxableAmount);
                                        saleOrderReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : saleOrderReportObj.Amount);
                                        saleOrderReportObj.PSAUser = new PSAUser();
                                        saleOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : saleOrderReportObj.PSAUser.LoginName);
                                        saleOrderReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : saleOrderReportObj.TotalCount);
                                        saleOrderReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : saleOrderReportObj.FilteredCount);
                                        saleOrderReportObj.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : saleOrderReportObj.Qty);
                                        //saleOrderReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : saleOrderReportObj.ProductSpec);
                                        saleOrderReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : saleOrderReportObj.ProductSpec);
                                        saleOrderReportObj.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : saleOrderReportObj.ProductID);
                                        saleOrderReportObj.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : saleOrderReportObj.ProductModelID);
                                        saleOrderReportObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : saleOrderReportObj.UnitCode);
                                        saleOrderReportObj.Product = new Product();
                                        saleOrderReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : saleOrderReportObj.Product.Name);
                                        saleOrderReportObj.ProductModel = new ProductModel();
                                        saleOrderReportObj.ProductModel.Name = (sdr["ProductModalName"].ToString() != "" ? sdr["ProductModalName"].ToString() : saleOrderReportObj.ProductModel.Name);
                                        saleOrderReportObj.Unit = new Unit();
                                        saleOrderReportObj.Unit.Description = (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : saleOrderReportObj.Unit.Description);
                                        saleOrderReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : saleOrderReportObj.TotalAmount);
                                        saleOrderReportObj.TotalTaxableAmount = (sdr["TotalTaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TotalTaxableAmount"].ToString()) : saleOrderReportObj.TotalTaxableAmount);
                                    }
                                    saleOrderReportList.Add(saleOrderReportObj);
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

            return saleOrderReportList;
        }
        #endregion GetSaleOrderStandardReport

        #region GetQuotationDetailReport
        public List<QuotationDetailReport> GetQuotationDetailReport(QuotationDetailReport quotationDetailReport)
        {

            List<QuotationDetailReport> quotationDetailReportList = null;
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
                        cmd.CommandText = "[PSA].[GetQuotationDetailReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(quotationDetailReport.SearchTerm) ? "" : quotationDetailReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = quotationDetailReport.DataTablePaging.Start;
                        if (quotationDetailReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = quotationDetailReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = quotationDetailReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = quotationDetailReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = quotationDetailReport.AdvCustomer;
                        if (quotationDetailReport.AdvPreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = quotationDetailReport.AdvPreparedBy;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = quotationDetailReport.AdvReferencePersonCode;
                        if (quotationDetailReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = quotationDetailReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = quotationDetailReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = quotationDetailReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = quotationDetailReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = quotationDetailReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = quotationDetailReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = quotationDetailReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = quotationDetailReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.Int).Value = quotationDetailReport.AdvApprovalStatusCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = quotationDetailReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = quotationDetailReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = quotationDetailReport.AdvReportType;
                        if (quotationDetailReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = quotationDetailReport.AdvProduct;
                        if (quotationDetailReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = quotationDetailReport.AdvProductModel;


                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                quotationDetailReportList = new List<QuotationDetailReport>();
                                while (sdr.Read())
                                {
                                    QuotationDetailReport quotationDetailReportObj = new QuotationDetailReport();
                                    {
                                        quotationDetailReportObj.QuotationNo = (sdr["QuotationNo"].ToString() != "" ? (sdr["QuotationNo"].ToString()) : quotationDetailReportObj.QuotationNo);
                                        quotationDetailReportObj.QuoteNo = (sdr["QuoteNo"].ToString() != "" ? (sdr["QuoteNo"].ToString()) : quotationDetailReportObj.QuoteNo);
                                        quotationDetailReportObj.QuoteDate = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()) : quotationDetailReportObj.QuoteDate);
                                        quotationDetailReportObj.QuoteDateFormatted = (sdr["QuoteDate"].ToString() != "" ? DateTime.Parse(sdr["QuoteDate"].ToString()).ToString(_settings.DateFormat) : quotationDetailReportObj.QuoteDateFormatted);
                                        quotationDetailReportObj.Customer = new Customer();
                                        quotationDetailReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : quotationDetailReportObj.Customer.CompanyName);
                                        quotationDetailReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : quotationDetailReportObj.Customer.ContactPerson);
                                        quotationDetailReportObj.Branch = new Branch();
                                        quotationDetailReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : quotationDetailReportObj.Branch.Description);
                                        quotationDetailReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : quotationDetailReportObj.Amount);
                                        quotationDetailReportObj.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : quotationDetailReportObj.TaxableAmount);
                                        quotationDetailReportObj.PSAUser = new PSAUser();
                                        quotationDetailReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : quotationDetailReportObj.PSAUser.LoginName);
                                        quotationDetailReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : quotationDetailReportObj.TotalCount);
                                        quotationDetailReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : quotationDetailReportObj.FilteredCount);
                                        quotationDetailReportObj.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : quotationDetailReportObj.Qty);
                                        //quotationDetailReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : quotationDetailReportObj.ProductSpec);
                                        quotationDetailReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : quotationDetailReportObj.ProductSpec);
                                        quotationDetailReportObj.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : quotationDetailReportObj.ProductID);
                                        quotationDetailReportObj.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : quotationDetailReportObj.ProductModelID);
                                        quotationDetailReportObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : quotationDetailReportObj.UnitCode);
                                        quotationDetailReportObj.Product = new Product();
                                        quotationDetailReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : quotationDetailReportObj.Product.Name);
                                        quotationDetailReportObj.ProductModel = new ProductModel();
                                        quotationDetailReportObj.ProductModel.Name = (sdr["ProductModalName"].ToString() != "" ? sdr["ProductModalName"].ToString() : quotationDetailReportObj.ProductModel.Name);
                                        quotationDetailReportObj.Unit = new Unit();
                                        quotationDetailReportObj.Unit.Description = (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : quotationDetailReportObj.Unit.Description);
                                        quotationDetailReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : quotationDetailReportObj.TotalAmount);
                                        quotationDetailReportObj.TotalTaxableAmount = (sdr["TotalTaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TotalTaxableAmount"].ToString()) : quotationDetailReportObj.TotalTaxableAmount);
                                    }
                                    quotationDetailReportList.Add(quotationDetailReportObj);
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

            return quotationDetailReportList;
        }
        #endregion GetQuotationDetailReport

        #region GetEnquiryDetailReport
        public List<EnquiryDetailReport> GetEnquiryDetailReport(EnquiryDetailReport enquiryDetailReport)
        {

            List<EnquiryDetailReport> enquiryDetailReportList = null;
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
                        cmd.CommandText = "[PSA].[GetEnquiryDetailReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(enquiryDetailReport.SearchTerm) ? "" : enquiryDetailReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = enquiryDetailReport.DataTablePaging.Start;
                        if (enquiryDetailReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = enquiryDetailReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = enquiryDetailReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = enquiryDetailReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = enquiryDetailReport.AdvCustomer;
                        if (enquiryDetailReport.AdvAttendedBy != Guid.Empty)
                            cmd.Parameters.Add("@AttendedBy", SqlDbType.UniqueIdentifier).Value = enquiryDetailReport.AdvAttendedBy;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = enquiryDetailReport.AdvReferencePersonCode;
                        if (enquiryDetailReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = enquiryDetailReport.AdvDocumentOwnerID;
                        if (enquiryDetailReport.AdvResponsibleBy != Guid.Empty)
                            cmd.Parameters.Add("@ResponsibleBy", SqlDbType.UniqueIdentifier).Value = enquiryDetailReport.AdvResponsibleBy;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = enquiryDetailReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = enquiryDetailReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = enquiryDetailReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = enquiryDetailReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = enquiryDetailReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = enquiryDetailReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = enquiryDetailReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@GradeCode", SqlDbType.Int).Value = enquiryDetailReport.AdvEnquiryGradeCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = enquiryDetailReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = enquiryDetailReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = enquiryDetailReport.AdvReportType;
                        if (enquiryDetailReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = enquiryDetailReport.AdvProduct;
                        if (enquiryDetailReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = enquiryDetailReport.AdvProductModel;


                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                enquiryDetailReportList = new List<EnquiryDetailReport>();
                                while (sdr.Read())
                                {
                                    EnquiryDetailReport enquiryDetailReportObj = new EnquiryDetailReport();
                                    {
                                        enquiryDetailReportObj.EnquiryNo = (sdr["EnquiryNo"].ToString() != "" ? (sdr["EnquiryNo"].ToString()) : enquiryDetailReportObj.EnquiryNo);
                                        enquiryDetailReportObj.EnqNo = (sdr["EnqNo"].ToString() != "" ? (sdr["EnqNo"].ToString()) : enquiryDetailReportObj.EnqNo);
                                        enquiryDetailReportObj.EnquiryDate = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()) : enquiryDetailReportObj.EnquiryDate);
                                        enquiryDetailReportObj.EnquiryDateFormatted = (sdr["EnquiryDate"].ToString() != "" ? DateTime.Parse(sdr["EnquiryDate"].ToString()).ToString(_settings.DateFormat) : enquiryDetailReportObj.EnquiryDateFormatted);
                                        enquiryDetailReportObj.Customer = new Customer();
                                        enquiryDetailReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : enquiryDetailReportObj.Customer.CompanyName);
                                        enquiryDetailReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : enquiryDetailReportObj.Customer.ContactPerson);
                                        enquiryDetailReportObj.Branch = new Branch();
                                        enquiryDetailReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : enquiryDetailReportObj.Branch.Description);
                                        enquiryDetailReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : enquiryDetailReportObj.Amount);
                                        enquiryDetailReportObj.PSAUser = new PSAUser();
                                        enquiryDetailReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : enquiryDetailReportObj.PSAUser.LoginName);
                                        enquiryDetailReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : enquiryDetailReportObj.TotalCount);
                                        enquiryDetailReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : enquiryDetailReportObj.FilteredCount);
                                        enquiryDetailReportObj.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : enquiryDetailReportObj.Qty);
                                        //enquiryDetailReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : enquiryDetailReportObj.ProductSpec);
                                        enquiryDetailReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : enquiryDetailReportObj.ProductSpec);
                                        enquiryDetailReportObj.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : enquiryDetailReportObj.ProductID);
                                        enquiryDetailReportObj.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : enquiryDetailReportObj.ProductModelID);
                                        enquiryDetailReportObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : enquiryDetailReportObj.UnitCode);
                                        enquiryDetailReportObj.Product = new Product();
                                        enquiryDetailReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : enquiryDetailReportObj.Product.Name);
                                        enquiryDetailReportObj.ProductModel = new ProductModel();
                                        enquiryDetailReportObj.ProductModel.Name = (sdr["ProductModalName"].ToString() != "" ? sdr["ProductModalName"].ToString() : enquiryDetailReportObj.ProductModel.Name);
                                        enquiryDetailReportObj.Unit = new Unit();
                                        enquiryDetailReportObj.Unit.Description = (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : enquiryDetailReportObj.Unit.Description);
                                        enquiryDetailReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : enquiryDetailReportObj.TotalAmount);
                                    }
                                    enquiryDetailReportList.Add(enquiryDetailReportObj);
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

            return enquiryDetailReportList;
        }
        #endregion GetEnquiryDetailReport

        #region GetProductionOrderStandardReport

        public List<ProductionOrderReport> GetProductionOrderStandardReport(ProductionOrderReport productionOrderReport)
        {

            List<ProductionOrderReport> productionOrderReportList = null;
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
                        cmd.CommandText = "[PSA].[GetProductionOrderStandardReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productionOrderReport.SearchTerm) ? "" : productionOrderReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionOrderReport.DataTablePaging.Start;
                        if (productionOrderReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionOrderReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = productionOrderReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = productionOrderReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = productionOrderReport.AdvCustomer;                                          
                        if (productionOrderReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionOrderReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionOrderReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = productionOrderReport.AdvAreaCode;
                       cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionOrderReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = productionOrderReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = productionOrderReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = productionOrderReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = productionOrderReport.AdvCustomerCategoryCode;                   
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = productionOrderReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = productionOrderReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = productionOrderReport.AdvReportType;
                        //cmd.Parameters.Add("@ExpDelDateFrom", SqlDbType.DateTime).Value = productionOrderReport.AdvDelFromDate;
                        //cmd.Parameters.Add("@ExpDelDateTo", SqlDbType.DateTime).Value = productionOrderReport.AdvDelToDate;
                        if (productionOrderReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productionOrderReport.AdvProduct;
                        if (productionOrderReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = productionOrderReport.AdvProductModel;
                        cmd.Parameters.Add("@Progress", SqlDbType.Int).Value = productionOrderReport.AdvProgress;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = productionOrderReport.AdvPlantCode;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = productionOrderReport.AdvReferencePersonCode;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionOrderReportList = new List<ProductionOrderReport>();
                                while (sdr.Read())
                                {
                                    ProductionOrderReport productionOrderReportObj = new ProductionOrderReport();
                                    {
                                        productionOrderReportObj.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? (sdr["ProdOrderNo"].ToString()) : productionOrderReportObj.ProdOrderNo);
                                        productionOrderReportObj.ProductionOrderNo = (sdr["ProductionOrdNo"].ToString() != "" ? (sdr["ProductionOrdNo"].ToString()) : productionOrderReportObj.ProductionOrderNo);
                                        
                                        productionOrderReportObj.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : productionOrderReportObj.ProdOrderDate);
                                        productionOrderReportObj.ProdOrderDateFormatted = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()).ToString(_settings.DateFormat) : productionOrderReportObj.ProdOrderDateFormatted);
                                        productionOrderReportObj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : productionOrderReportObj.SaleOrderNo);
                                        productionOrderReportObj.SaleOrdNo = (sdr["SaleOrdNo"].ToString() != "" ? (sdr["SaleOrdNo"].ToString()) : productionOrderReportObj.SaleOrdNo);
                                        productionOrderReportObj.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : productionOrderReportObj.ExpectedDelvDate);
                                        productionOrderReportObj.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : productionOrderReportObj.ExpectedDelvDateFormatted);
                                        productionOrderReportObj.PreparedBy= (sdr["PreparedByName"].ToString() != "" ? sdr["PreparedByName"].ToString() : productionOrderReportObj.PreparedBy);

                                        productionOrderReportObj.Area = new Area();
                                        productionOrderReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : productionOrderReportObj.Area.Description);
                                        productionOrderReportObj.DocumentStatus = new DocumentStatus();
                                        productionOrderReportObj.DocumentStatus.Description = (sdr["DocumentStatusName"].ToString() != "" ? sdr["DocumentStatusName"].ToString() : productionOrderReportObj.DocumentStatus.Description);
                                        productionOrderReportObj.Remarks= (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionOrderReportObj.Remarks);
                                        productionOrderReportObj.Customer = new Customer();
                                        productionOrderReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : productionOrderReportObj.Customer.CompanyName);
                                        productionOrderReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : productionOrderReportObj.Customer.ContactPerson);
                                        productionOrderReportObj.Branch = new Branch();
                                        productionOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : productionOrderReportObj.Branch.Description);
                                        productionOrderReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : productionOrderReportObj.Amount);
                                        productionOrderReportObj.PSAUser = new PSAUser();
                                        productionOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : productionOrderReportObj.PSAUser.LoginName);
                                        productionOrderReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionOrderReportObj.TotalCount);
                                        productionOrderReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productionOrderReportObj.FilteredCount);
                                        productionOrderReportObj.Qty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : productionOrderReportObj.Qty);

                                        productionOrderReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : productionOrderReportObj.ProductSpec);
                                       
                                        productionOrderReportObj.Product = new Product();
                                        productionOrderReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : productionOrderReportObj.Product.Name);
                                        productionOrderReportObj.ProductModel = new ProductModel();
                                        productionOrderReportObj.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? sdr["ProductModelName"].ToString() : productionOrderReportObj.ProductModel.Name);
                                        productionOrderReportObj.Plant = new Plant();
                                        productionOrderReportObj.Plant.Description= (sdr["PlantName"].ToString() != "" ? sdr["PlantName"].ToString() : productionOrderReportObj.Plant.Description);
                                        productionOrderReportObj.ReferencePerson = new ReferencePerson();
                                        productionOrderReportObj.ReferencePerson.Name= (sdr["ReferedByName"].ToString() != "" ? sdr["ReferedByName"].ToString() : productionOrderReportObj.ReferencePerson.Name);
                                        productionOrderReportObj.Branch = new Branch();
                                        productionOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : productionOrderReportObj.Branch.Description);
                                        productionOrderReportObj.PSAUser = new PSAUser();
                                        productionOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : productionOrderReportObj.PSAUser.LoginName);
                                        productionOrderReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : productionOrderReportObj.TotalAmount);
                                    }
                                    productionOrderReportList.Add(productionOrderReportObj);
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

            return productionOrderReportList;
        }


        #endregion GetProductionOrderStandardReport

        #region GetProductionOrderDetailForecastDateExceededReport

        public List<ProductionOrderDetailForecastDateExceededReport> GetProductionOrderDetailForecastDateExceededReport(ProductionOrderDetailForecastDateExceededReport productionOrderdetailForecastDateReport)
        {

            List<ProductionOrderDetailForecastDateExceededReport> productionOrderDetailForecastDateExceededReportList = null;
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
                        cmd.CommandText = "[PSA].[GetProductionOrderDetailForecastDateExceeded]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productionOrderdetailForecastDateReport.SearchTerm) ? "" : productionOrderdetailForecastDateReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.DataTablePaging.Start;
                        if (productionOrderdetailForecastDateReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = DateTime.Today.ToString("dd-MMM-yyyy");
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = productionOrderdetailForecastDateReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = productionOrderdetailForecastDateReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = productionOrderdetailForecastDateReport.AdvCustomer;
                        if (productionOrderdetailForecastDateReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionOrderdetailForecastDateReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = productionOrderdetailForecastDateReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = productionOrderdetailForecastDateReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvReportType;
                        cmd.Parameters.Add("@ReportValue", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.ReportValue;
                        //cmd.Parameters.Add("@ExpDelDateFrom", SqlDbType.DateTime).Value = productionOrderReport.AdvDelFromDate;
                        //cmd.Parameters.Add("@ExpDelDateTo", SqlDbType.DateTime).Value = productionOrderReport.AdvDelToDate;
                        if (productionOrderdetailForecastDateReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productionOrderdetailForecastDateReport.AdvProduct;
                        if (productionOrderdetailForecastDateReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = productionOrderdetailForecastDateReport.AdvProductModel;
                        cmd.Parameters.Add("@Progress", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvProgress;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvPlantCode;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = productionOrderdetailForecastDateReport.AdvReferencePersonCode;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionOrderDetailForecastDateExceededReportList = new List<ProductionOrderDetailForecastDateExceededReport>();
                                while (sdr.Read())
                                {
                                    ProductionOrderDetailForecastDateExceededReport productionOrderReportObj = new ProductionOrderDetailForecastDateExceededReport();
                                    {
                                        productionOrderReportObj.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? (sdr["ProdOrderNo"].ToString()) : productionOrderReportObj.ProdOrderNo);
                                        productionOrderReportObj.ProductionOrderNo = (sdr["ProductionOrdNo"].ToString() != "" ? (sdr["ProductionOrdNo"].ToString()) : productionOrderReportObj.ProductionOrderNo);

                                        productionOrderReportObj.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : productionOrderReportObj.ProdOrderDate);
                                        productionOrderReportObj.ProdOrderDateFormatted = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()).ToString(_settings.DateFormat) : productionOrderReportObj.ProdOrderDateFormatted);
                                        //productionOrderReportObj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : productionOrderReportObj.SaleOrderNo);
                                        //productionOrderReportObj.SaleOrdNo = (sdr["SaleOrdNo"].ToString() != "" ? (sdr["SaleOrdNo"].ToString()) : productionOrderReportObj.SaleOrdNo);
                                        productionOrderReportObj.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : productionOrderReportObj.ExpectedDelvDate);
                                        productionOrderReportObj.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : productionOrderReportObj.ExpectedDelvDateFormatted);
                                        productionOrderReportObj.ExptCompletionDate = (sdr["ExptCompletionDate"].ToString() != "" ? DateTime.Parse(sdr["ExptCompletionDate"].ToString()).ToString(_settings.DateFormat) : productionOrderReportObj.ExptCompletionDate);
                                        productionOrderReportObj.PreparedBy = (sdr["PreparedByName"].ToString() != "" ? sdr["PreparedByName"].ToString() : productionOrderReportObj.PreparedBy);

                                        productionOrderReportObj.Area = new Area();
                                        productionOrderReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : productionOrderReportObj.Area.Description);
                                        productionOrderReportObj.DocumentStatus = new DocumentStatus();
                                        productionOrderReportObj.DocumentStatus.Description = (sdr["DocumentStatusName"].ToString() != "" ? sdr["DocumentStatusName"].ToString() : productionOrderReportObj.DocumentStatus.Description);
                                        productionOrderReportObj.Remarks = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionOrderReportObj.Remarks);
                                        productionOrderReportObj.Customer = new Customer();
                                        productionOrderReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : productionOrderReportObj.Customer.CompanyName);
                                        productionOrderReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : productionOrderReportObj.Customer.ContactPerson);
                                        productionOrderReportObj.Branch = new Branch();
                                        productionOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : productionOrderReportObj.Branch.Description);
                                        productionOrderReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : productionOrderReportObj.Amount);
                                        productionOrderReportObj.PSAUser = new PSAUser();
                                        productionOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : productionOrderReportObj.PSAUser.LoginName);
                                        productionOrderReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionOrderReportObj.TotalCount);
                                        productionOrderReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productionOrderReportObj.FilteredCount);
                                        productionOrderReportObj.Qty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : productionOrderReportObj.Qty);
                                        productionOrderReportObj.ProducedQty = (sdr["ProducedQty"].ToString() != "" ? decimal.Parse(sdr["ProducedQty"].ToString()) : productionOrderReportObj.Qty);
                                        productionOrderReportObj.Progress = (sdr["progressPerc"].ToString() != "" ? int.Parse(sdr["progressPerc"].ToString()) : productionOrderReportObj.Progress);
                                        productionOrderReportObj.ExptProgress = (sdr["ExptProgressPerc"].ToString() != "" ? int.Parse(sdr["ExptProgressPerc"].ToString()) : productionOrderReportObj.Progress);
                                        productionOrderReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : productionOrderReportObj.ProductSpec);

                                        productionOrderReportObj.Product = new Product();
                                        productionOrderReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : productionOrderReportObj.Product.Name);
                                        productionOrderReportObj.ProductModel = new ProductModel();
                                        productionOrderReportObj.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? sdr["ProductModelName"].ToString() : productionOrderReportObj.ProductModel.Name);
                                        productionOrderReportObj.Plant = new Plant();
                                        productionOrderReportObj.Plant.Description = (sdr["PlantName"].ToString() != "" ? sdr["PlantName"].ToString() : productionOrderReportObj.Plant.Description);
                                        productionOrderReportObj.ReferencePerson = new ReferencePerson();
                                        //productionOrderReportObj.ReferencePerson.Name = (sdr["ReferedByName"].ToString() != "" ? sdr["ReferedByName"].ToString() : productionOrderReportObj.ReferencePerson.Name);
                                        productionOrderReportObj.Branch = new Branch();
                                        productionOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : productionOrderReportObj.Branch.Description);
                                        productionOrderReportObj.PSAUser = new PSAUser();
                                        productionOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : productionOrderReportObj.PSAUser.LoginName);
                                        productionOrderReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : productionOrderReportObj.TotalAmount);
                                    }
                                    productionOrderDetailForecastDateExceededReportList.Add(productionOrderReportObj);
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

            return productionOrderDetailForecastDateExceededReportList;
        }


        #endregion GetProductionOrderDetailForecastDateExceededReport



        #region GetPendingProductionOrderReport
        public List<PendingProductionOrderReport> GetPendingProductionOrderReport(PendingProductionOrderReport pendingProductionOrderReport)
        {

            List<PendingProductionOrderReport> pendingProductionOrderReportList = null;
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
                        cmd.CommandText = "[PSA].[GetPendingProductionOrderReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(pendingProductionOrderReport.SearchTerm) ? "" : pendingProductionOrderReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = pendingProductionOrderReport.DataTablePaging.Start;
                        if (pendingProductionOrderReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = pendingProductionOrderReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = pendingProductionOrderReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = pendingProductionOrderReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = pendingProductionOrderReport.AdvCustomer;
                        if (pendingProductionOrderReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = pendingProductionOrderReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = pendingProductionOrderReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = pendingProductionOrderReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = pendingProductionOrderReport.AdvReportType;
                        //cmd.Parameters.Add("@ExpDelDateFrom", SqlDbType.DateTime).Value = productionOrderReport.AdvDelFromDate;
                        //cmd.Parameters.Add("@ExpDelDateTo", SqlDbType.DateTime).Value = productionOrderReport.AdvDelToDate;
                        if (pendingProductionOrderReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = pendingProductionOrderReport.AdvProduct;
                        if (pendingProductionOrderReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = pendingProductionOrderReport.AdvProductModel;
                        cmd.Parameters.Add("@Progress", SqlDbType.Int).Value = pendingProductionOrderReport.AdvProgress;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = pendingProductionOrderReport.AdvPlantCode;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = pendingProductionOrderReport.AdvReferencePersonCode;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                pendingProductionOrderReportList = new List<PendingProductionOrderReport>();
                                while (sdr.Read())
                                {
                                    PendingProductionOrderReport pendingProductionOrderReportObj = new PendingProductionOrderReport();
                                    {
                                        pendingProductionOrderReportObj.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? (sdr["ProdOrderNo"].ToString()) : pendingProductionOrderReportObj.ProdOrderNo);
                                        pendingProductionOrderReportObj.ProductionOrderNo = (sdr["ProductionOrdNo"].ToString() != "" ? (sdr["ProductionOrdNo"].ToString()) : pendingProductionOrderReportObj.ProductionOrderNo);

                                        pendingProductionOrderReportObj.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : pendingProductionOrderReportObj.ProdOrderDate);
                                        pendingProductionOrderReportObj.ProdOrderDateFormatted = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()).ToString(_settings.DateFormat) : pendingProductionOrderReportObj.ProdOrderDateFormatted);

                                        pendingProductionOrderReportObj.ForecastDate = (sdr["MileStone4FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone4FcFinishDt"].ToString()) : pendingProductionOrderReportObj.ForecastDate);
                                        pendingProductionOrderReportObj.ForecastDateFormatted = (sdr["MileStone4FcFinishDt"].ToString() != "" ? DateTime.Parse(sdr["MileStone4FcFinishDt"].ToString()).ToString(_settings.DateFormat) : pendingProductionOrderReportObj.ForecastDateFormatted);


                                        pendingProductionOrderReportObj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : pendingProductionOrderReportObj.SaleOrderNo);
                                        pendingProductionOrderReportObj.SaleOrdNo = (sdr["SaleOrdNo"].ToString() != "" ? (sdr["SaleOrdNo"].ToString()) : pendingProductionOrderReportObj.SaleOrdNo);
                                        pendingProductionOrderReportObj.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : pendingProductionOrderReportObj.ExpectedDelvDate);
                                        pendingProductionOrderReportObj.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : pendingProductionOrderReportObj.ExpectedDelvDateFormatted);
                                        pendingProductionOrderReportObj.PreparedBy = (sdr["PreparedByName"].ToString() != "" ? sdr["PreparedByName"].ToString() : pendingProductionOrderReportObj.PreparedBy);

                                        pendingProductionOrderReportObj.Area = new Area();
                                        pendingProductionOrderReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : pendingProductionOrderReportObj.Area.Description);
                                        pendingProductionOrderReportObj.DocumentStatus = new DocumentStatus();
                                        pendingProductionOrderReportObj.DocumentStatus.Description = (sdr["DocumentStatusName"].ToString() != "" ? sdr["DocumentStatusName"].ToString() : pendingProductionOrderReportObj.DocumentStatus.Description);
                                        pendingProductionOrderReportObj.Remarks = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : pendingProductionOrderReportObj.Remarks);
                                        pendingProductionOrderReportObj.Customer = new Customer();
                                        pendingProductionOrderReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : pendingProductionOrderReportObj.Customer.CompanyName);
                                        pendingProductionOrderReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : pendingProductionOrderReportObj.Customer.ContactPerson);
                                        pendingProductionOrderReportObj.Branch = new Branch();
                                        pendingProductionOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : pendingProductionOrderReportObj.Branch.Description);
                                        pendingProductionOrderReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : pendingProductionOrderReportObj.Amount);
                                        pendingProductionOrderReportObj.PSAUser = new PSAUser();
                                        pendingProductionOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : pendingProductionOrderReportObj.PSAUser.LoginName);
                                        pendingProductionOrderReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : pendingProductionOrderReportObj.TotalCount);
                                        pendingProductionOrderReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : pendingProductionOrderReportObj.FilteredCount);
                                        pendingProductionOrderReportObj.Qty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : pendingProductionOrderReportObj.Qty);
                                        pendingProductionOrderReportObj.PendingQty= (sdr["PendingQty"].ToString() != "" ? decimal.Parse(sdr["PendingQty"].ToString()) : pendingProductionOrderReportObj.PendingQty);
                                        pendingProductionOrderReportObj.SaleOrderQty= (sdr["SaleOrderQty"].ToString() != "" ? decimal.Parse(sdr["SaleOrderQty"].ToString()) : pendingProductionOrderReportObj.SaleOrderQty);
                                        pendingProductionOrderReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : pendingProductionOrderReportObj.ProductSpec);
                                        pendingProductionOrderReportObj.Progress = (sdr["progressPerc"].ToString() != "" ? int.Parse(sdr["progressPerc"].ToString()) : pendingProductionOrderReportObj.Progress);

                                        pendingProductionOrderReportObj.Product = new Product();
                                        pendingProductionOrderReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : pendingProductionOrderReportObj.Product.Name);
                                        pendingProductionOrderReportObj.ProductModel = new ProductModel();
                                        pendingProductionOrderReportObj.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? sdr["ProductModelName"].ToString() : pendingProductionOrderReportObj.ProductModel.Name);
                                        pendingProductionOrderReportObj.Plant = new Plant();
                                        pendingProductionOrderReportObj.Plant.Description = (sdr["PlantName"].ToString() != "" ? sdr["PlantName"].ToString() : pendingProductionOrderReportObj.Plant.Description);
                                        pendingProductionOrderReportObj.ReferencePerson = new ReferencePerson();
                                        pendingProductionOrderReportObj.ReferencePerson.Name = (sdr["ReferedByName"].ToString() != "" ? sdr["ReferedByName"].ToString() : pendingProductionOrderReportObj.ReferencePerson.Name);
                                        pendingProductionOrderReportObj.Branch = new Branch();
                                        pendingProductionOrderReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : pendingProductionOrderReportObj.Branch.Description);
                                        pendingProductionOrderReportObj.PSAUser = new PSAUser();
                                        pendingProductionOrderReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : pendingProductionOrderReportObj.PSAUser.LoginName);
                                        pendingProductionOrderReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : pendingProductionOrderReportObj.TotalAmount);
                                    }
                                    pendingProductionOrderReportList.Add(pendingProductionOrderReportObj);
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

            return pendingProductionOrderReportList;
        }
        #endregion GetPendingProductionOrderReport


        #region GetProductionQCStandardReport
        public List<ProductionQCStandardReport> GetProductionQCStandardReport(ProductionQCStandardReport productionQCStandardReport)
        {

            List<ProductionQCStandardReport> productionQCStandardReportList = null;
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
                        cmd.CommandText = "[PSA].[GetProductionQCStandardReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productionQCStandardReport.SearchTerm) ? "" : productionQCStandardReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionQCStandardReport.DataTablePaging.Start;
                        if (productionQCStandardReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionQCStandardReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = productionQCStandardReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = productionQCStandardReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = productionQCStandardReport.AdvCustomer;
                        if (productionQCStandardReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionQCStandardReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionQCStandardReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = productionQCStandardReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionQCStandardReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = productionQCStandardReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = productionQCStandardReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = productionQCStandardReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = productionQCStandardReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = productionQCStandardReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = productionQCStandardReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = productionQCStandardReport.AdvReportType;                      
                        if (productionQCStandardReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productionQCStandardReport.AdvProduct;
                        if (productionQCStandardReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = productionQCStandardReport.AdvProductModel;                      
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = productionQCStandardReport.AdvPlantCode;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = productionQCStandardReport.AdvReferencePersonCode;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionQCStandardReportList = new List<ProductionQCStandardReport>();
                                while (sdr.Read())
                                {
                                    ProductionQCStandardReport productionQCReportObj = new ProductionQCStandardReport();
                                    {
                                        productionQCReportObj.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? (sdr["ProdOrderNo"].ToString()) : productionQCReportObj.ProdOrderNo);
                                        productionQCReportObj.ProductionOrderNo = (sdr["ProductionOrdNo"].ToString() != "" ? (sdr["ProductionOrdNo"].ToString()) : productionQCReportObj.ProductionOrderNo);

                                        productionQCReportObj.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : productionQCReportObj.ProdOrderDate);
                                        productionQCReportObj.ProdOrderDateFormatted = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()).ToString(_settings.DateFormat) : productionQCReportObj.ProdOrderDateFormatted);
                                        productionQCReportObj.ProdQCNo = (sdr["ProdQCNos"].ToString() != "" ? (sdr["ProdQCNos"].ToString()) : productionQCReportObj.ProdQCNo);
                                        productionQCReportObj.ProductionQCNo = (sdr["ProductionQCNo"].ToString() != "" ? (sdr["ProductionQCNo"].ToString()) : productionQCReportObj.ProductionQCNo);
                                        productionQCReportObj.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : productionQCReportObj.ExpectedDelvDate);
                                        productionQCReportObj.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : productionQCReportObj.ExpectedDelvDateFormatted);
                                        //productionQCReportObj.PreparedBy = (sdr["PreparedByName"].ToString() != "" ? sdr["PreparedByName"].ToString() : productionQCReportObj.PreparedBy);

                                        productionQCReportObj.Area = new Area();
                                        productionQCReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : productionQCReportObj.Area.Description);
                                        productionQCReportObj.DocumentStatus = new DocumentStatus();
                                        productionQCReportObj.DocumentStatus.Description = (sdr["DocumentStatusName"].ToString() != "" ? sdr["DocumentStatusName"].ToString() : productionQCReportObj.DocumentStatus.Description);
                                        productionQCReportObj.Remarks = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : productionQCReportObj.Remarks);
                                        productionQCReportObj.Customer = new Customer();
                                        productionQCReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : productionQCReportObj.Customer.CompanyName);
                                        productionQCReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : productionQCReportObj.Customer.ContactPerson);
                                        productionQCReportObj.Branch = new Branch();
                                        productionQCReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : productionQCReportObj.Branch.Description);
                                        productionQCReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : productionQCReportObj.Amount);
                                        productionQCReportObj.PSAUser = new PSAUser();
                                        productionQCReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : productionQCReportObj.PSAUser.LoginName);
                                        productionQCReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionQCReportObj.TotalCount);
                                        productionQCReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productionQCReportObj.FilteredCount);
                                      //  productionQCReportObj.Qty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : productionQCReportObj.Qty);
                                        productionQCReportObj.ProdOrdQty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : productionQCReportObj.ProdOrdQty);
                                        productionQCReportObj.ProdQCQty = (sdr["QCQty"].ToString() != "" ? decimal.Parse(sdr["QCQty"].ToString()) : productionQCReportObj.ProdQCQty);
                                        productionQCReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : productionQCReportObj.ProductSpec);
                                       // productionQCReportObj.Progress = (sdr["progressPerc"].ToString() != "" ? int.Parse(sdr["progressPerc"].ToString()) : productionQCReportObj.Progress);

                                        productionQCReportObj.Product = new Product();
                                        productionQCReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : productionQCReportObj.Product.Name);
                                        productionQCReportObj.ProductModel = new ProductModel();
                                        productionQCReportObj.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? sdr["ProductModelName"].ToString() : productionQCReportObj.ProductModel.Name);
                                        productionQCReportObj.Plant = new Plant();
                                        productionQCReportObj.Plant.Description = (sdr["PlantName"].ToString() != "" ? sdr["PlantName"].ToString() : productionQCReportObj.Plant.Description);
                                        productionQCReportObj.ReferencePerson = new ReferencePerson();
                                        productionQCReportObj.ReferencePerson.Name = (sdr["ReferedByName"].ToString() != "" ? sdr["ReferedByName"].ToString() : productionQCReportObj.ReferencePerson.Name);
                                        productionQCReportObj.Branch = new Branch();
                                        productionQCReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : productionQCReportObj.Branch.Description);
                                        productionQCReportObj.PSAUser = new PSAUser();
                                        productionQCReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : productionQCReportObj.PSAUser.LoginName);
                                        productionQCReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : productionQCReportObj.TotalAmount);
                                    }
                                    productionQCStandardReportList.Add(productionQCReportObj);
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

            return productionQCStandardReportList;
        }
        #endregion GetProductionQCStandardReport

        #region GetPendingProductionQCReport
        public List<PendingProductionQCReport> GetPendingProductionQCReport(PendingProductionQCReport productionQCStandardReport)
        {

            List<PendingProductionQCReport> pendingroductionQCReportList = null;
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
                        cmd.CommandText = "[PSA].[GetPendingProductionQCReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productionQCStandardReport.SearchTerm) ? "" : productionQCStandardReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionQCStandardReport.DataTablePaging.Start;
                        if (productionQCStandardReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionQCStandardReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = productionQCStandardReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = productionQCStandardReport.AdvToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = productionQCStandardReport.AdvCustomer;
                        if (productionQCStandardReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = productionQCStandardReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = productionQCStandardReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = productionQCStandardReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = productionQCStandardReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = productionQCStandardReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = productionQCStandardReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = productionQCStandardReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = productionQCStandardReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@AmountFrom", SqlDbType.Decimal).Value = productionQCStandardReport.AdvAmountFrom;
                        cmd.Parameters.Add("@AmountTo", SqlDbType.Decimal).Value = productionQCStandardReport.AdvAmountTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = productionQCStandardReport.AdvReportType;
                        if (productionQCStandardReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productionQCStandardReport.AdvProduct;
                        if (productionQCStandardReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = productionQCStandardReport.AdvProductModel;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.Int).Value = productionQCStandardReport.AdvPlantCode;
                        cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = productionQCStandardReport.AdvReferencePersonCode;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                pendingroductionQCReportList = new List<PendingProductionQCReport>();
                                while (sdr.Read())
                                {
                                    PendingProductionQCReport pendingProductionQCReportObj = new PendingProductionQCReport();
                                    {
                                        pendingProductionQCReportObj.ProdOrderNo = (sdr["ProdOrderNo"].ToString() != "" ? (sdr["ProdOrderNo"].ToString()) : pendingProductionQCReportObj.ProdOrderNo);
                                        pendingProductionQCReportObj.ProductionOrderNo = (sdr["ProductionOrdNo"].ToString() != "" ? (sdr["ProductionOrdNo"].ToString()) : pendingProductionQCReportObj.ProductionOrderNo);

                                        pendingProductionQCReportObj.ProdOrderDate = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()) : pendingProductionQCReportObj.ProdOrderDate);
                                        pendingProductionQCReportObj.ProdOrderDateFormatted = (sdr["ProdOrderDate"].ToString() != "" ? DateTime.Parse(sdr["ProdOrderDate"].ToString()).ToString(_settings.DateFormat) : pendingProductionQCReportObj.ProdOrderDateFormatted);
                                        pendingProductionQCReportObj.ProdQCNo = (sdr["ProdQCNos"].ToString() != "" ? (sdr["ProdQCNos"].ToString()) : pendingProductionQCReportObj.ProdQCNo);
                                        pendingProductionQCReportObj.ProductionQCNo = (sdr["ProductionQCNo"].ToString() != "" ? (sdr["ProductionQCNo"].ToString()) : pendingProductionQCReportObj.ProductionQCNo);
                                        pendingProductionQCReportObj.ExpectedDelvDate = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()) : pendingProductionQCReportObj.ExpectedDelvDate);
                                        pendingProductionQCReportObj.ExpectedDelvDateFormatted = (sdr["ExpectedDelvDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDelvDate"].ToString()).ToString(_settings.DateFormat) : pendingProductionQCReportObj.ExpectedDelvDateFormatted);
                                      
                                        pendingProductionQCReportObj.Area = new Area();
                                        pendingProductionQCReportObj.Area.Description = (sdr["AreaName"].ToString() != "" ? sdr["AreaName"].ToString() : pendingProductionQCReportObj.Area.Description);
                                        pendingProductionQCReportObj.DocumentStatus = new DocumentStatus();
                                        pendingProductionQCReportObj.DocumentStatus.Description = (sdr["DocumentStatusName"].ToString() != "" ? sdr["DocumentStatusName"].ToString() : pendingProductionQCReportObj.DocumentStatus.Description);
                                        pendingProductionQCReportObj.Remarks = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : pendingProductionQCReportObj.Remarks);
                                        pendingProductionQCReportObj.Customer = new Customer();
                                        pendingProductionQCReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : pendingProductionQCReportObj.Customer.CompanyName);
                                        pendingProductionQCReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : pendingProductionQCReportObj.Customer.ContactPerson);
                                        pendingProductionQCReportObj.Branch = new Branch();
                                        pendingProductionQCReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : pendingProductionQCReportObj.Branch.Description);
                                        pendingProductionQCReportObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : pendingProductionQCReportObj.Amount);
                                        pendingProductionQCReportObj.PSAUser = new PSAUser();
                                        pendingProductionQCReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : pendingProductionQCReportObj.PSAUser.LoginName);
                                        pendingProductionQCReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : pendingProductionQCReportObj.TotalCount);
                                        pendingProductionQCReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : pendingProductionQCReportObj.FilteredCount);
                                        pendingProductionQCReportObj.PendingQty = (sdr["PendingQty"].ToString() != "" ? decimal.Parse(sdr["PendingQty"].ToString()) : pendingProductionQCReportObj.PendingQty);
                                        pendingProductionQCReportObj.ProdOrdQty = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : pendingProductionQCReportObj.ProdOrdQty);
                                        pendingProductionQCReportObj.ProdQCQty = (sdr["QCQty"].ToString() != "" ? decimal.Parse(sdr["QCQty"].ToString()) : pendingProductionQCReportObj.ProdQCQty);
                                        pendingProductionQCReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : pendingProductionQCReportObj.ProductSpec);
                                    
                                        pendingProductionQCReportObj.Product = new Product();
                                        pendingProductionQCReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : pendingProductionQCReportObj.Product.Name);
                                        pendingProductionQCReportObj.ProductModel = new ProductModel();
                                        pendingProductionQCReportObj.ProductModel.Name = (sdr["ProductModelName"].ToString() != "" ? sdr["ProductModelName"].ToString() : pendingProductionQCReportObj.ProductModel.Name);
                                        pendingProductionQCReportObj.Plant = new Plant();
                                        pendingProductionQCReportObj.Plant.Description = (sdr["PlantName"].ToString() != "" ? sdr["PlantName"].ToString() : pendingProductionQCReportObj.Plant.Description);
                                        pendingProductionQCReportObj.ReferencePerson = new ReferencePerson();
                                        pendingProductionQCReportObj.ReferencePerson.Name = (sdr["ReferedByName"].ToString() != "" ? sdr["ReferedByName"].ToString() : pendingProductionQCReportObj.ReferencePerson.Name);
                                        pendingProductionQCReportObj.Branch = new Branch();
                                        pendingProductionQCReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : pendingProductionQCReportObj.Branch.Description);
                                        pendingProductionQCReportObj.PSAUser = new PSAUser();
                                        pendingProductionQCReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : pendingProductionQCReportObj.PSAUser.LoginName);
                                        pendingProductionQCReportObj.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? decimal.Parse(sdr["TotalAmount"].ToString()) : pendingProductionQCReportObj.TotalAmount);
                                    }
                                    pendingroductionQCReportList.Add(pendingProductionQCReportObj);
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

            return pendingroductionQCReportList;
        }
        #endregion GetPendingProductionQCReport

        #region GetEstimateDetailReport
        public List<EstimateDetailReport> GetEstimateDetailReport(EstimateDetailReport estimateDetailReport)
        {

            List<EstimateDetailReport> estimateDetailReportList = null;
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
                        cmd.CommandText = "[PSA].[GetEstimateDetailReport]";
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(estimateDetailReport.SearchTerm) ? "" : estimateDetailReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = estimateDetailReport.DataTablePaging.Start;
                        if (estimateDetailReport.DataTablePaging.Length == -1)
                        {
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = estimateDetailReport.DataTablePaging.Length;
                        }
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = estimateDetailReport.AdvFromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = estimateDetailReport.AdvToDate;
                        cmd.Parameters.Add("@ValidDateFrom", SqlDbType.DateTime).Value = estimateDetailReport.AdvValidFromDate;
                        cmd.Parameters.Add("@ValidDateTo", SqlDbType.DateTime).Value = estimateDetailReport.AdvValidToDate;
                        cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, 100).Value = estimateDetailReport.AdvCustomer;
                        if (estimateDetailReport.AdvPreparedBy != Guid.Empty)
                            cmd.Parameters.Add("@PreparedBy", SqlDbType.UniqueIdentifier).Value = estimateDetailReport.AdvPreparedBy;
                        if (estimateDetailReport.AdvDocumentOwnerID != Guid.Empty)
                            cmd.Parameters.Add("@DocumentOwnerID", SqlDbType.UniqueIdentifier).Value = estimateDetailReport.AdvDocumentOwnerID;
                        cmd.Parameters.Add("@DocumentStatusCode", SqlDbType.Int).Value = estimateDetailReport.AdvDocumentStatusCode;
                        cmd.Parameters.Add("@CustAreaCode", SqlDbType.Int).Value = estimateDetailReport.AdvAreaCode;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.Int).Value = estimateDetailReport.AdvBranchCode;
                        cmd.Parameters.Add("@CustCountryCode", SqlDbType.Int).Value = estimateDetailReport.AdvCountryCode;
                        cmd.Parameters.Add("@CustStateCode", SqlDbType.Int).Value = estimateDetailReport.AdvStateCode;
                        cmd.Parameters.Add("@CustDistCode", SqlDbType.Int).Value = estimateDetailReport.AdvDistrictCode;
                        cmd.Parameters.Add("@CustCategoryCode", SqlDbType.Int).Value = estimateDetailReport.AdvCustomerCategoryCode;
                        cmd.Parameters.Add("@TotalCostRateFrom", SqlDbType.Decimal).Value = estimateDetailReport.AdvTotalCostRateFrom;
                        cmd.Parameters.Add("@TotalCostRateTo", SqlDbType.Decimal).Value = estimateDetailReport.AdvTotalCostRateTo;
                        cmd.Parameters.Add("@TotalSellingRateFrom", SqlDbType.Decimal).Value = estimateDetailReport.AdvTotalSellingRateFrom;
                        cmd.Parameters.Add("@TotalSellingRateTo", SqlDbType.Decimal).Value = estimateDetailReport.AdvTotalSellingRateTo;
                        cmd.Parameters.Add("@ReportType", SqlDbType.Int).Value = estimateDetailReport.AdvReportType;
                        if (estimateDetailReport.AdvProduct != Guid.Empty)
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = estimateDetailReport.AdvProduct;
                        if (estimateDetailReport.AdvProductModel != Guid.Empty)
                            cmd.Parameters.Add("@ProductModelID", SqlDbType.UniqueIdentifier).Value = estimateDetailReport.AdvProductModel;


                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                estimateDetailReportList = new List<EstimateDetailReport>();
                                while (sdr.Read())
                                {
                                    EstimateDetailReport estimateDetailReportObj = new EstimateDetailReport();
                                    {
                                        estimateDetailReportObj.EstimateNo = (sdr["EstimateNo"].ToString() != "" ? (sdr["EstimateNo"].ToString()) : estimateDetailReportObj.EstimateNo);
                                        estimateDetailReportObj.EstNo = (sdr["EstNo"].ToString() != "" ? (sdr["EstNo"].ToString()) : estimateDetailReportObj.EstNo);
                                        estimateDetailReportObj.EstimateDate = (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()) : estimateDetailReportObj.EstimateDate);
                                        estimateDetailReportObj.EstimateDateFormatted = (sdr["EstimateDate"].ToString() != "" ? DateTime.Parse(sdr["EstimateDate"].ToString()).ToString(_settings.DateFormat) : estimateDetailReportObj.EstimateDateFormatted);
                                        estimateDetailReportObj.Customer = new Customer();
                                        estimateDetailReportObj.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : estimateDetailReportObj.Customer.CompanyName);
                                        estimateDetailReportObj.Customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : estimateDetailReportObj.Customer.ContactPerson);
                                        estimateDetailReportObj.Branch = new Branch();
                                        estimateDetailReportObj.Branch.Description = (sdr["BranchName"].ToString() != "" ? sdr["BranchName"].ToString() : estimateDetailReportObj.Branch.Description);
                                        estimateDetailReportObj.TotalCostRate = (sdr["TotalCostRate"].ToString() != "" ? decimal.Parse(sdr["TotalCostRate"].ToString()) : estimateDetailReportObj.TotalCostRate);
                                        estimateDetailReportObj.TotalSellingRate = (sdr["TotalSellingRate"].ToString() != "" ? decimal.Parse(sdr["TotalSellingRate"].ToString()) : estimateDetailReportObj.TotalSellingRate);
                                        estimateDetailReportObj.PSAUser = new PSAUser();
                                        estimateDetailReportObj.PSAUser.LoginName = (sdr["DocumentOwnerName"].ToString() != "" ? sdr["DocumentOwnerName"].ToString() : estimateDetailReportObj.PSAUser.LoginName);
                                        estimateDetailReportObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : estimateDetailReportObj.TotalCount);
                                        estimateDetailReportObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : estimateDetailReportObj.FilteredCount);
                                        estimateDetailReportObj.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : estimateDetailReportObj.Qty);
                                        //estimateDetailReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString() : estimateDetailReportObj.ProductSpec);
                                        estimateDetailReportObj.ProductSpec = (sdr["ProductSpec"].ToString() != "" ? sdr["ProductSpec"].ToString().Replace("$n$", "\n") : estimateDetailReportObj.ProductSpec);
                                        estimateDetailReportObj.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : estimateDetailReportObj.ProductID);
                                        estimateDetailReportObj.ProductModelID = (sdr["ProductModelID"].ToString() != "" ? Guid.Parse(sdr["ProductModelID"].ToString()) : estimateDetailReportObj.ProductModelID);
                                        estimateDetailReportObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? int.Parse(sdr["UnitCode"].ToString()) : estimateDetailReportObj.UnitCode);
                                        estimateDetailReportObj.Product = new Product();
                                        estimateDetailReportObj.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : estimateDetailReportObj.Product.Name);
                                        estimateDetailReportObj.ProductModel = new ProductModel();
                                        estimateDetailReportObj.ProductModel.Name = (sdr["ProductModalName"].ToString() != "" ? sdr["ProductModalName"].ToString() : estimateDetailReportObj.ProductModel.Name);
                                        estimateDetailReportObj.Unit = new Unit();
                                        estimateDetailReportObj.Unit.Description = (sdr["Unit"].ToString() != "" ? sdr["Unit"].ToString() : estimateDetailReportObj.Unit.Description);
                                        estimateDetailReportObj.TotalCostAmount = (sdr["TotalCostAmount"].ToString() != "" ? decimal.Parse(sdr["TotalCostAmount"].ToString()) : estimateDetailReportObj.TotalCostAmount);
                                        estimateDetailReportObj.TotalSellingAmount = (sdr["TotalSellingAmount"].ToString() != "" ? decimal.Parse(sdr["TotalSellingAmount"].ToString()) : estimateDetailReportObj.TotalSellingAmount);
                                    }
                                    estimateDetailReportList.Add(estimateDetailReportObj);
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

            return estimateDetailReportList;
        }
        #endregion GetEstimateDetailReport

    }
}
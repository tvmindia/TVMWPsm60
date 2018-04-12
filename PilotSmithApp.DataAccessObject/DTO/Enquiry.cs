﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Enquiry
    {
        public Guid ID { get; set; }
        public string EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string RequirementSpec { get; set; }
        public Guid? CustomerID { get; set; }
        public int? EnquiryGradeCode { get; set; }
        public int? DocumentStatusCode { get; set; }
        public int? ReferredByCode { get; set; }
        public Guid? ResponsiblePersonID { get; set; }
        public Guid? AttendedByID { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }
        //Additional properties
        public string EnquiryDateFormatted { get; set;}
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Customer Customer { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public List<EnquiryDetail> EnquiryDetailList { get; set; }
    }
    public class EnquiryAdvanceSearch
    {
        public string EnquiryDate { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
    public class EnquiryDetail
    {
        public Guid ID { get; set; }
        public Guid EnquiryID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public Product Product { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }
}

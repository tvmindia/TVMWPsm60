using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Estimate
    {
        public Guid ID { get; set; }
        public string EstimateNo { get; set; }
        public string EstimateRefNo { get; set; }
        public DateTime EstimateDate { get; set; }
        public Guid? EnquiryID { get; set; }
        public Guid? CustomerID { get; set; }
        public int? DocumentStatusCode { get; set; }
        public DateTime ValidUpToDate { get; set; }
        public Guid? PreparedBy { get; set; }
        public string GeneralNotes { get; set; }
        public Guid? DocumentOwnerID { get; set; }
        public int? BranchCode { get; set; }

        //Aditional Fields
        public string DetailXML { get; set; }
        public string EstimateDateFormatted { get; set; }
        public string ValidUpToDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }

    public class EstimateAdvanceSearch
    {
        public string EstimateDate { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }

    public class EstimateDetail
    {
        public Guid ID { get; set; }
        public Guid EstimateID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? CostRate { get; set; }
        public decimal? SellingRate { get; set; }
        public string DrawingNo { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }
}

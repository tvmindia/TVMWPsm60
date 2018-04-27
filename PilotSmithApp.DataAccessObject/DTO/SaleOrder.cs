using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class SaleOrder
    {
        public Guid ID { get; set; }
        public Guid SaleOrderID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        public int? TaxTypeCode { get; set; }
        public decimal? CGSTAmt { get; set; }
        public decimal? SGSTAmt { get; set; }
        public decimal? IGSTAmt { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? CessAmt { get; set; }

        //Additional Fields
        public PSASysCommon PSASysCommon { get; set; }
        public string DetailXML { get; set; }
        public Guid hdnFileID { get; set; }
        public List<SaleOrderDetail> SaleOrderDetailList { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Customer Customer { get; set; }
        public string LatestApprovalStatusDescription { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public Branch Branch { get; set; }
    }

    public class SaleOrderAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }

    public class SaleOrderDetail
    {
        public Guid ID { get; set; }
        public Guid SaleOrderID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? ProductModelID { get; set; }
        public string ProductSpec { get; set; }
        public decimal? Qty { get; set; }
        public int? UnitCode { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Discount { get; set; }
        public int? TaxTypeCode { get; set; }
        public decimal? CGSTAmt { get; set; }
        public decimal? SGSTAmt { get; set; }
        public decimal? IGSTAmt { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? CessAmt { get; set; }

        public PSASysCommon PSASysCommon { get; set; }
        public Product Product { get; set; }
        public ProductModel ProductModel { get; set; }
        public Unit Unit { get; set; }
    }
}

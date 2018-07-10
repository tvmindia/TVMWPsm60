using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class ProductModel
    {
        public Guid ID { get; set; }
        public Guid? ProductID { get; set; }
        public string Name { get; set; }
        public int? UnitCode { get; set; }
        public string Specification { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public DateTime IntroducedDate { get; set; }
        public decimal? StockQty { get; set; }
        public string ImageURL { get; set; }

        //Additional Fields
        public string IntroducedDateFormatted { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Product Product { get; set; }
        public Unit Unit { get; set; }
        public string ProductModelSelect { get; set; }
    }

    public class ProductModelAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}

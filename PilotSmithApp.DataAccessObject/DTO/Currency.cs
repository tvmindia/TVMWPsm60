using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class Currency
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string DisplayInWords { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
        public decimal CurrencyRate { get; set; }
        public bool IsUpdate { get; set; }
        public string DocumentType { get; set; }
        public Guid DocumentID { get; set; }
        public string DocumentNo { get; set; }
    }
}

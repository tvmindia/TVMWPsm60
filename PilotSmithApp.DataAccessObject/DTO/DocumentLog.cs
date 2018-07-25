using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class DocumentLog
    {
        public int Code { get; set; }
        public string DocumentNo { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Remarks { get; set; }
        public PSASysCommon PSASysCommon { get; set; }
    }
}

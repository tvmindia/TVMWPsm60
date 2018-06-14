using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class TimeLine
    {
        public String DocumentType { get; set; }
        public Guid DocumentID { get; set; }
        public String DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string URL { get; set; }
    }
}

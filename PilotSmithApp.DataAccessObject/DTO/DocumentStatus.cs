using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.DataAccessObject.DTO
{
    public class DocumentStatus
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string DocumentTypeCode { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}

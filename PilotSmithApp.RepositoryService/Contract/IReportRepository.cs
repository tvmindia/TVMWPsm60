using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
   public interface IReportRepository
    {
        List<PSASysReport> GetAllReport(string searchTerm);
       // List<PendingSaleOrderProductionReport> GetPendingSaleOrderProductionReport(PendingSaleOrderProductionReport pendingSaleOrderProductionReport);
        List<EnquiryReport> GetEnquiryReport(EnquiryReport enquiryReport);
    }
}

using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IEstimateRepository
    {
        List<Estimate> GetAllEstimate(EstimateAdvanceSearch estimateAdvanceSearch);
        Estimate GetEstimate(Guid id);
        List<EstimateDetail> GetEstimateDetailListByEstimateID(Guid estimateID);
        List<Estimate> GetEstimateForSelectList(Guid? id);
        List<Estimate> GetEstimateForSelectListOnDemand(string searchTerm);
        object InsertUpdateEstimate(Estimate estimate);
        object DeleteEstimate(Guid id);
        object DeleteEstimateDetail(Guid id);
    }
}

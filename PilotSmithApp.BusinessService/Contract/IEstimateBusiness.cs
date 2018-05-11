using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IEstimateBusiness
    {
        List<Estimate> GetAllEstimate(EstimateAdvanceSearch estimateAdvanceSearch);
        Estimate GetEstimate(Guid id);
        List<EstimateDetail> GetEstimateDetailListByEstimateID(Guid estimateID);
        List<SelectListItem> GetEstimateForSelectList(Guid? id);
        List<Estimate> GetEstimateForSelectListOnDemand(string searchTerm);
        object InsertUpdateEstimate(Estimate estimate);
        object DeleteEstimate(Guid id);
        object DeleteEstimateDetail(Guid id);
    }
}

using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class EstimateBusiness:IEstimateBusiness
    {
        private IEstimateRepository _estimateRepository;
        ICommonBusiness _commonBusiness;
        public EstimateBusiness(IEstimateRepository estimateRepository, ICommonBusiness commonBusiness)
        {
            _estimateRepository = estimateRepository;
            _commonBusiness = commonBusiness;
        }
        public List<Estimate> GetAllEstimate(EstimateAdvanceSearch estimateAdvanceSearch)
        {
            return _estimateRepository.GetAllEstimate(estimateAdvanceSearch);
        }
        public Estimate GetEstimate(Guid id)
        {
            return _estimateRepository.GetEstimate(id);
        }
        public List<EstimateDetail> GetEstimateDetailListByEstimateID(Guid estimateID)
        {

            return _estimateRepository.GetEstimateDetailListByEstimateID(estimateID);
        }
        public object InsertUpdateEstimate(Estimate estimate)
        {
            if (estimate.EstimateDetailList.Count > 0)
            {
                estimate.DetailXML = _commonBusiness.GetXMLfromEstimateObject(estimate.EstimateDetailList, "ProductID");
            }
            return _estimateRepository.InsertUpdateEstimate(estimate);
        }
        public object DeleteEstimate(Guid id)
        {
            return _estimateRepository.DeleteEstimate(id);
        }
        public object DeleteEstimateDetail(Guid id)
        {
            return _estimateRepository.DeleteEstimateDetail(id);
        }
        public List<SelectListItem> GetEstimateForSelectList()
        {
            List<SelectListItem> selectListItem =null;
            List<Estimate> estimateList = _estimateRepository.GetEstimateForSelectList();
            return selectListItem = estimateList!=null?(from estimate in estimateList
                                     select new SelectListItem
                                     {
                                         Text = estimate.EstimateNo,
                                         Value = estimate.ID.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
    }  
}

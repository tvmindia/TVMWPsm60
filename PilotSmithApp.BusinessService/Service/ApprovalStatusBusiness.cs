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
   public class ApprovalStatusBusiness:IApprovalStatusBusiness
    {
        IApprovalStatusRepository _approvalStatusRepository;
        public ApprovalStatusBusiness(IApprovalStatusRepository approvalStatusRepository)
        {
            _approvalStatusRepository = approvalStatusRepository;
        }

        public List<SelectListItem> GetSelectListForApprovalStatus()
        {
            List<SelectListItem> selectListItem = null;
            List<ApprovalStatus> approvalStatusList = _approvalStatusRepository.GetApprovalStatusSelectList();
            return selectListItem = approvalStatusList != null ? (from approvalStatus in approvalStatusList
                                                                  select new SelectListItem
                                                                  {
                                                                      Text = approvalStatus.Description,
                                                                      Value = approvalStatus.Code.ToString(),
                                                                      Selected = false                                                                      
                                                                  }).ToList() : new List<SelectListItem>();
        }

    }
}

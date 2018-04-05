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
    public class BranchBusiness : IBranchBusiness
    {
        IBranchRepository _branchRepository;
        public BranchBusiness(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }
        public List<SelectListItem> GetBranchForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Branch> branchList = _branchRepository.GetBranchForSelectList();
            if (branchList != null)
                foreach (Branch branch in branchList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = branch.Description,
                        Value = branch.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}

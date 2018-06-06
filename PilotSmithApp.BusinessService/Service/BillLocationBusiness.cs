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
    public class BillLocationBusiness: IBillLocationBusiness
    {
        IBillLocationRepository _billLocationRepository;
        public BillLocationBusiness(IBillLocationRepository billLocationRepository)
        {
            _billLocationRepository = billLocationRepository;
        }
        public List<SelectListItem> GetBillLocationForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<BillLocation> billLocationList = _billLocationRepository.GetBillLocationForSelectList();
            return selectListItem = billLocationList != null ? (from billLocation in billLocationList
                                                          select new SelectListItem
                                                          {
                                                              Text = billLocation.Name,
                                                              Value = billLocation.Code.ToString(),
                                                              Selected = false,//billLocation.UserInBillLocation.IsDefault ? true : false,
                                                          }).ToList() : new List<SelectListItem>();
        }
    }
}

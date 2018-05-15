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
    public class OtherChargeBusiness : IOtherChargeBusiness
    {
        private IOtherChargeRepository _otherChargeRepository;
        public OtherChargeBusiness(IOtherChargeRepository otherChargeRepository)
        {
            _otherChargeRepository = otherChargeRepository;
        }
        public object InsertUpdateOtherCharge(OtherCharge otherCharge)
        {
            return _otherChargeRepository.InsertUpdateOtherCharge(otherCharge);
        }
        public List<OtherCharge> GetAllOtherCharge(OtherChargeAdvanceSearch otherChargeAdvanceSearch)
        {
            return _otherChargeRepository.GetAllOtherCharge(otherChargeAdvanceSearch);
        }
        public OtherCharge GetOtherCharge(int code)
        {
            return _otherChargeRepository.GetOtherCharge(code);
        }
        public bool CheckOtherChargeCodeExist(OtherCharge otherCharge)
        {
            return _otherChargeRepository.CheckOtherChargeCodeExist(otherCharge);
        }
        public object DeleteOtherCharge(int code)
        {
            return _otherChargeRepository.DeleteOtherCharge(code);
        }
        public List<SelectListItem> GetOtherChargeForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<OtherCharge> otherChargeList = _otherChargeRepository.GetOtherChargeForSelectList();
            return selectListItem = otherChargeList != null ? (from otherCharge in otherChargeList
                                                         select new SelectListItem
                                                         {
                                                             Text = otherCharge.Description,
                                                             Value = otherCharge.Code.ToString(),
                                                             Selected = false
                                                         }).ToList() : new List<SelectListItem>();
        }
    }
}

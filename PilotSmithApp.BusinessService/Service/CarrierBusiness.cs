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
    public class CarrierBusiness : ICarrierBusiness
    {
        private ICarrierRepository _carrierRepository;
        public CarrierBusiness(ICarrierRepository carrierRepository)
        {
            _carrierRepository = carrierRepository;
        }
        public List<SelectListItem> GetCarrierForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Carrier> carrierList = _carrierRepository.GetCarrierForSelectList();
            return selectListItem = carrierList != null ? (from carrier in carrierList
                                                           select new SelectListItem
                                                        {
                                                            Text = carrier.Name,
                                                            Value = carrier.Code.ToString(),
                                                            Selected = false
                                                        }).ToList() : new List<SelectListItem>();
        }
    }
}

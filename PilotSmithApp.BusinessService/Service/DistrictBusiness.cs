﻿using PilotSmithApp.BusinessService.Contract;
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
    public class DistrictBusiness:IDistrictBusiness
    {
        private IDistrictRepository _districtRepository;
        public DistrictBusiness(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }
        public object InsertUpdateDistrict(District district)
        {
            return _districtRepository.InsertUpdateDistrict(district);
        }
        public List<District> GetAllDistrict(DistrictAdvanceSearch districtAdvanceSearch)
        {
            return _districtRepository.GetAllDistrict(districtAdvanceSearch);
        }
        public District GetDistrict(int code)
        {
            return _districtRepository.GetDistrict(code);
        }
        public bool CheckDistrictNameExist(District district)
        {
            return _districtRepository.CheckDistrictNameExist(district);
        }
        public object DeleteDistrict(int code)
        {
            return _districtRepository.DeleteDistrict(code);
        }
        public List<SelectListItem> GetDistrictForSelectList(int? stateCode, int? countryCode)
        {
            List<SelectListItem> selectListItem = null;
            List<District> districtList = _districtRepository.GetDistrictForSelectList(stateCode, countryCode);
            return selectListItem = districtList!=null?(from district in districtList
                                     select new SelectListItem
                                     {
                                         Text = district.Description,
                                         Value = district.Code.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
    }
}

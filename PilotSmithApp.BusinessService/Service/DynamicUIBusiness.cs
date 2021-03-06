﻿using PilotSmithApp.BusinessService.Contracts;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class DynamicUIBusiness:IDynamicUIBusiness
    {
        private IDynamicUIRepository _dynamicUIRepository;
        public DynamicUIBusiness(IDynamicUIRepository dynamicUIRespository)
        {
            _dynamicUIRepository = dynamicUIRespository;

        }
        public List<PSASysMenu> GetAllMenues()
        {
            return _dynamicUIRepository.GetAllMenues();
        }
        public Boolean IsFileExisting(Guid id, string doctype)
        {
            return _dynamicUIRepository.IsFileExisting(id,doctype);
        }
    }
}

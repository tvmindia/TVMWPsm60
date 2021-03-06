﻿using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface ISysSettingBusiness
    {
        List<SysSetting> GetAllSysSetting(SysSettingAdvanceSearch sysSettingAdvanceSearch);
        object InsertUpdateSysSetting(SysSetting sysSetting);
        SysSetting GetSysSetting(Guid id);
        object DeleteSysSetting(Guid id);
    }
}

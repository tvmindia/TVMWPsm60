using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class SysSettingBusiness: ISysSettingBusiness
    {

        private ISysSettingRepository _sysSettingRepository;
        public SysSettingBusiness(ISysSettingRepository sysSettingRepoitory)
        {
            _sysSettingRepository = sysSettingRepoitory;
        }

        public List<SysSetting> GetAllSysSetting(SysSettingAdvanceSearch sysSettingAdvanceSearch)
        {
            return _sysSettingRepository.GetAllSysSetting(sysSettingAdvanceSearch);
        }

        public object InsertUpdateSysSetting(SysSetting sysSetting)
        {
            return _sysSettingRepository.InsertUpdateSysSetting(sysSetting);
        }

        public SysSetting GetSysSetting(Guid id)
        {
            return _sysSettingRepository.GetSysSetting(id);
        }

        public object DeleteSysSetting(Guid id)
        {
            return _sysSettingRepository.DeleteSysSetting(id);
        }

    }
}

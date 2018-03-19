using PilotSmithApp.DataAccessObject.DTO; 
using System.Collections.Generic;
 

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IDynamicUIRepository
    {
        List<PSASysMenu> GetAllMenues();
    }
}

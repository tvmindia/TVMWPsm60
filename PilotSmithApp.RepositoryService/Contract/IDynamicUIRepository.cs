using PilotSmithApp.DataAccessObject.DTO; 
using System.Collections.Generic;
 

namespace PilotSmithApp.RepositoryServices.Contracts
{
    public interface IDynamicUIRepository
    {
        List<PSASysMenu> GetAllMenues();
    }
}

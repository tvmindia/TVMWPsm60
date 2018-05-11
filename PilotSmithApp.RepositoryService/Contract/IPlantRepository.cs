using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IPlantRepository
    {
        object InsertUpdatePlant(Plant plant);
        List<Plant> GetAllPlant(PlantAdvanceSearch plantAdvanceSearch);
        Plant GetPlant(int code);
        bool CheckPlantNameExist(Plant plant);
        object DeletePlant(int code);
        List<Plant> GetPlantForSelectList();
    }
}

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
    public class PlantBusiness: IPlantBusiness
    {
        IPlantRepository _plantRepository;
        public PlantBusiness(IPlantRepository plantRepository)
        {
            _plantRepository = plantRepository;
        }
        public object InsertUpdatePlant(Plant plant)
        {
            return _plantRepository.InsertUpdatePlant(plant);
        }
        public List<Plant> GetAllPlant(PlantAdvanceSearch plantAdvanceSearch)
        {
            return _plantRepository.GetAllPlant(plantAdvanceSearch);
        }
        public Plant GetPlant(int code)
        {
            return _plantRepository.GetPlant(code);
        }
        public bool CheckPlantNameExist(Plant plant)
        {
            return _plantRepository.CheckPlantNameExist(plant);
        }
        public object DeletePlant(int code)
        {
            return _plantRepository.DeletePlant(code);
        }
        public List<SelectListItem> GetPlantForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Plant> plantList = _plantRepository.GetPlantForSelectList();
            return selectListItem = (from plant in plantList
                                     select new SelectListItem
                                     {
                                         Text = plant.Description,
                                         Value = plant.Code.ToString(),
                                         Selected = false
                                     }).ToList();
        }
    }
}

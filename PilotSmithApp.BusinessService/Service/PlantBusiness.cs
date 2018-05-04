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
        public List<SelectListItem> GetPlantForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Plant> plantList = _plantRepository.GetPlantForSelectList();
            if (plantList != null)
                foreach (Plant plant in plantList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = plant.Description,
                        Value = plant.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}

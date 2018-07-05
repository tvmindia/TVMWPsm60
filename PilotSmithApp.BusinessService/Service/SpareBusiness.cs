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
    public class SpareBusiness: ISpareBusiness
    {
        private ISpareRepository _spareRepository;
        public SpareBusiness(ISpareRepository spareRepoitory)
        {
            _spareRepository = spareRepoitory;
        }

        public List<Spare> GetAllSpare(SpareAdvanceSearch spareAdvanceSearch)
        {
            return _spareRepository.GetAllSpare(spareAdvanceSearch);
        }
        public object DeleteSpare(Guid id)
        {
            return _spareRepository.DeleteSpare(id);
        }
        public object InsertUpdateSpare(Spare spare)
        {
            return _spareRepository.InsertUpdateSpare(spare);
        }
        public string GetSpareCode()
        {
            return _spareRepository.GetSpareCode();
        }
        public bool CheckSpareCodeExist(Spare spare)
        {
            return _spareRepository.CheckSpareCodeExist(spare);
        }
        public Spare GetSpare(Guid id)
        {
            return _spareRepository.GetSpare(id);
        }
        public List<SelectListItem> GetSpareForSelectList()
        {
            List<SelectListItem> selectListItem = null;
            List<Spare> spareList = _spareRepository.GetSpareForSelectList();
            return selectListItem = spareList != null ? (from spare in spareList
                                                           select new SelectListItem
                                                           {
                                                               Text = spare.Code + " - " + spare.Name,
                                                               Value = spare.ID.ToString(),
                                                               Selected = false
                                                           }).ToList() : new List<SelectListItem>();
        }
    }
}

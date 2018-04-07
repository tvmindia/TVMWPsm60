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
    public class StateBusiness:IStateBusiness
    {
        private IStateRepository _stateRepository;
        public StateBusiness(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }
        public object InsertUpdateState(State state)
        {
            return _stateRepository.InsertUpdateState(state);
        }
        public List<State> GetAllState(StateAdvanceSearch stateAdvanceSearch)
        {
            return _stateRepository.GetAllState(stateAdvanceSearch);
        }
        public State GetState(int code)
        {
            return _stateRepository.GetState(code);
        }
        public bool CheckStateNameExist(State state)
        {
            return _stateRepository.CheckStateNameExist(state);
        }
        public object DeleteState(int code)
        {
            return _stateRepository.DeleteState(code);
        }
        public List<SelectListItem> GetStateForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<State> stateList = _stateRepository.GetStateForSelectList();
            if (stateList != null)
                foreach (State state in stateList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = state.Description,
                        Value = state.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }

    }
}

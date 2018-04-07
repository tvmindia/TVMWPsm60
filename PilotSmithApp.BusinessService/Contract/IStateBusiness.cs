using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IStateBusiness
    {
        object InsertUpdateState(State state);
        List<State> GetAllState(StateAdvanceSearch stateAdvanceSearch);
        State GetState(int code);
        bool CheckStateNameExist(int code);
        object DeleteState(int code);
        List<SelectListItem> GetStateForSelectList();
    }
}

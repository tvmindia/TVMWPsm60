using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IStateRepository
    {
        object InsertUpdateState(State state);
        List<State> GetAllState(StateAdvanceSearch stateAdvanceSearch);
        State GetState(int code);
        bool CheckStateNameExist(State state);
        object DeleteState(int code);
        List<State> GetStateForSelectList(int? countryCode);
    }
}

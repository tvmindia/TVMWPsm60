using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IProductionQCBusiness
    {
        List<ProductionQC> GetAllProductionQC(ProductionQCAdvanceSearch productionQCAdvanceSearch);
        ProductionQC GetProductionQC(Guid id);
        List<ProductionQCDetail> GetProductionQCDetailListByProductionQCID(Guid productionQCID);
        object InsertUpdateProductionQC(ProductionQC productionQC);
        object DeleteProductionQC(Guid id);
        object DeleteProductionQCDetail(Guid id);
    }
}

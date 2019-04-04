using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IProductModelBusiness
    {
        object InsertUpdateProductModel(ProductModel productModel);
        List<ProductModel> GetAllProductModel(ProductModelAdvanceSearch productModelAdvanceSearch);
        ProductModel GetProductModel(Guid id, bool isMaster);
        bool CheckProductModelNameExist(ProductModel productModel);
        object DeleteProductModel(Guid id);
        List<SelectListItem> GetProductModelForSelectList(Guid productID);
        List<SelectListItem> GetProductModelSelectList();
    }
}

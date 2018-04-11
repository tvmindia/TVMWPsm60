using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IProductModelRepository
    {
        object InsertUpdateProductModel(ProductModel productModel);
        List<ProductModel> GetAllProductModel(ProductModelAdvanceSearch productModelAdvanceSearch);
        ProductModel GetProductModel(Guid id);
        bool CheckProductModelNameExist(ProductModel productModel);
        object DeleteProductModel(Guid id);
        List<ProductModel> GetProductModelForSelectList();
    }
}

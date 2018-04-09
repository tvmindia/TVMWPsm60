using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IProductCategoryRepository
    {
        object InsertUpdateProductCategory(ProductCategory productCategory);
        List<ProductCategory> GetAllProductCategory(ProductCategoryAdvanceSearch productCategoryAdvanceSearch);
        ProductCategory GetProductCategory(int code);
        bool CheckProductCategoryExist(ProductCategory productCategory);
        object DeleteProductCategory(int code);
        List<ProductCategory> GetProductCategoryForSelectList();
    }
}

using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IProductRepository
    {
        object InsertUpdateProduct(Product product);
        List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch);
        Product GetProduct(Guid id);
        bool CheckProductCodeExist(Product product);
        object DeleteProduct(Guid id);
        List<Product> GetProductForSelectList();
    }
}

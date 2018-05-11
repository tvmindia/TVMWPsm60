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
    public class ProductModelBusiness:IProductModelBusiness
    {
        private IProductModelRepository _prductModelRepository;
        public ProductModelBusiness(IProductModelRepository productModelRepoitory)
        {
            _prductModelRepository = productModelRepoitory;
        }
        public object InsertUpdateProductModel(ProductModel productModel)
        {
            return _prductModelRepository.InsertUpdateProductModel(productModel);
        }
        public List<ProductModel> GetAllProductModel(ProductModelAdvanceSearch productModelAdvanceSearch)
        {
            return _prductModelRepository.GetAllProductModel(productModelAdvanceSearch);
        }
        public ProductModel GetProductModel(Guid id)
        {
            return _prductModelRepository.GetProductModel(id);
        }
        public bool CheckProductModelNameExist(ProductModel productModel)
        {
            return _prductModelRepository.CheckProductModelNameExist(productModel);
        }
        public object DeleteProductModel(Guid id)
        {
            return _prductModelRepository.DeleteProductModel(id);
        }
        public List<SelectListItem> GetProductModelForSelectList(Guid productID)
        {
            List<SelectListItem> selectListItem = null;
            List<ProductModel> productModelList = _prductModelRepository.GetProductModelForSelectList(productID);
            return selectListItem = productModelList!=null?(from productModel in productModelList
                                     select new SelectListItem
                                     {
                                         Text = productModel.Name,
                                         Value = productModel.ID.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
    }
}

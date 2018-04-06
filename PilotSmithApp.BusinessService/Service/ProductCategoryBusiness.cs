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
    public class ProductCategoryBusiness:IProductCategoryBusiness
    {
        private IProductCategoryRepository _productCategoryRepository;
        public ProductCategoryBusiness(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public object InsertUpdateProductCategory(ProductCategory productCategory)
        {
            return _productCategoryRepository.InsertUpdateProductCategory(productCategory);
        }

        public List<ProductCategory>GetAllProductCategory(ProductCategoryAdvanceSearch productCategoryAdvanceSearch)
        {
            return _productCategoryRepository.GetAllProductCategory(productCategoryAdvanceSearch);
        }

        public ProductCategory GetProductCategory(int code)
        {
            return _productCategoryRepository.GetProductCategory(code);
        }

        public bool CheckProductCategoryCodeExist(int code)
        {
            return _productCategoryRepository.CheckProductCategoryCodeExist(code);
        }

        public object DeleteProductCategory(int code)
        {
            return _productCategoryRepository.DeleteProductCategory(code);
        }

        public List<SelectListItem> GetProductCategoryForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();           
            List<ProductCategory> productCategoryList = _productCategoryRepository.GetProductCategoryForSelectList();
            if (productCategoryList != null)
                foreach (ProductCategory productCategory in productCategoryList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = productCategory.Description,
                        Value = productCategory.Code.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}

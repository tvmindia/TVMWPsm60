﻿using PilotSmithApp.BusinessService.Contract;
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
        public List<SelectListItem> GetProductModelForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ProductModel> productModelList = _prductModelRepository.GetProductModelForSelectList();
            if (productModelList != null)
                foreach (ProductModel productModel in productModelList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = productModel.Name,
                        Value = productModel.ID.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}

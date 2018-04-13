using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;

namespace PilotSmithApp.UserInterface.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //domain <===== viewmodel
                //viewmodel =====> domain
                //ReverseMap() makes it possible to map both ways.


                //*****SAMTOOL MODELS 
                config.CreateMap<LoginViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<UserViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<SysMenuViewModel, SAMTool.DataAccessObject.DTO.SysMenu>().ReverseMap();
                config.CreateMap<RolesViewModel, SAMTool.DataAccessObject.DTO.Roles>().ReverseMap();
                config.CreateMap<ApplicationViewModel, SAMTool.DataAccessObject.DTO.Application>().ReverseMap();
                config.CreateMap<AppObjectViewModel, SAMTool.DataAccessObject.DTO.AppObject>().ReverseMap();
                config.CreateMap<AppSubobjectViewmodel, SAMTool.DataAccessObject.DTO.AppSubobject>().ReverseMap();
                config.CreateMap<PSASysCommonViewModel, SAMTool.DataAccessObject.DTO.Common>().ReverseMap();
                config.CreateMap<ManageAccessViewModel, SAMTool.DataAccessObject.DTO.ManageAccess>().ReverseMap();
                config.CreateMap<ManageSubObjectAccessViewModel, SAMTool.DataAccessObject.DTO.ManageSubObjectAccess>().ReverseMap();
                config.CreateMap<PrivilegesViewModel, SAMTool.DataAccessObject.DTO.Privileges>().ReverseMap();


                //****SAMTOOL MODELS END

                //****PilotSmithApp APP MODELS
                config.CreateMap<CustomerViewModel,Customer>().ReverseMap();
                config.CreateMap<CustomerAdvanceSearchViewModel, CustomerAdvanceSearch>().ReverseMap();
                config.CreateMap<TitlesViewModel, Titles>().ReverseMap();
                config.CreateMap<PaymentTermViewModel, PaymentTerm>().ReverseMap();
                config.CreateMap<PSASysCommonViewModel, PSASysCommon>().ReverseMap();
                config.CreateMap<ProductCategoryViewModel, ProductCategory>().ReverseMap();
                config.CreateMap<ProductCategoryAdvanceSearchViewModel, ProductCategoryAdvanceSearch>().ReverseMap();
                config.CreateMap<ProductSpecificationViewModel, ProductSpecification>().ReverseMap();
                config.CreateMap<ProductSpecificationAdvanceSearchViewModel, ProductSpecificationAdvanceSearch>().ReverseMap();
                config.CreateMap<StateViewModel, State>().ReverseMap();
                config.CreateMap<StateAdvanceSearchViewModel, StateAdvanceSearch>().ReverseMap();
                config.CreateMap<DistrictViewModel, District>().ReverseMap();
                config.CreateMap<DistrictAdvanceSearchViewModel, DistrictAdvanceSearch>().ReverseMap();
                config.CreateMap<AreaViewModel, Area>().ReverseMap();
                config.CreateMap<AreaAdvanceSearchViewModel, AreaAdvanceSearch>().ReverseMap();
                config.CreateMap<ProductViewModel, Product>().ReverseMap();
                config.CreateMap<ProductAdvanceSearchViewModel, ProductAdvanceSearch>().ReverseMap();
                config.CreateMap<CompanyViewModel, Company>().ReverseMap();
                config.CreateMap<CompanyAdvanceSearchViewModel, CompanyAdvanceSearch>().ReverseMap();
                config.CreateMap<ProductModelViewModel, ProductModel>().ReverseMap();
                config.CreateMap<ProductModelAdvanceSearchViewModel, ProductModelAdvanceSearch>().ReverseMap();
                config.CreateMap<UnitViewModel, Unit>().ReverseMap();
                config.CreateMap<UnitAdvanceSearchViewModel, UnitAdvanceSearch>().ReverseMap();
            });
        }
    }
}
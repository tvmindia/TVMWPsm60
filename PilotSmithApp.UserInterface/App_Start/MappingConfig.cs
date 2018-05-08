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
                config.CreateMap<EnquiryViewModel, Enquiry>().ReverseMap();
                config.CreateMap<EnquiryDetailViewModel, EnquiryDetail>().ReverseMap();
                config.CreateMap<EnquiryAdvanceSearchViewModel, EnquiryAdvanceSearch>().ReverseMap();
                config.CreateMap<EnquiryFollowupViewModel, EnquiryFollowup>().ReverseMap();
                config.CreateMap<EnquiryGradeViewModel, EnquiryGrade>().ReverseMap();
                config.CreateMap<ReferencePersonViewModel, ReferencePerson>().ReverseMap();
                config.CreateMap<ReferenceTypeViewModel, ReferenceType>().ReverseMap();
                config.CreateMap<DocumentStatusViewModel, DocumentStatus>().ReverseMap();
                config.CreateMap<EstimateViewModel, Estimate>().ReverseMap();
                config.CreateMap<EstimateDetailViewModel, EstimateDetail>().ReverseMap();
                config.CreateMap<EstimateAdvanceSearchViewModel, EstimateAdvanceSearch>().ReverseMap();
                config.CreateMap<BranchViewModel, Branch>().ReverseMap();
                config.CreateMap<EmployeeViewModel, Employee>().ReverseMap();
                config.CreateMap<EmployeeAdvanceSearchViewModel, EnquiryAdvanceSearch>().ReverseMap();
                config.CreateMap<PSAUserViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<QuotationViewModel, Quotation>().ReverseMap();
                config.CreateMap<QuotationAdvanceSearchViewModel, QuotationAdvanceSearch>().ReverseMap();
                config.CreateMap<QuotationDetailViewModel, QuotationDetail>().ReverseMap();
                config.CreateMap<PSAUserViewModel, PSAUser>().ReverseMap();
                config.CreateMap<SalesSummaryViewModel, SalesSummary>().ReverseMap();
                config.CreateMap<EnquiryValueFolloupSummaryViewModel, EnquiryValueFolloupSummary>().ReverseMap();
                config.CreateMap<TaxTypeViewModel, TaxType>().ReverseMap();
                config.CreateMap<DocumentApprovalViewModel, DocumentApproval>().ReverseMap();
                config.CreateMap<DocumentApprovalAdvanceSearchViewModel, DocumentApprovalAdvanceSearch>().ReverseMap();
                config.CreateMap<ApproverViewModel, Approver>().ReverseMap();
                config.CreateMap<ApproverAdvanceSearchViewModel, ApproverAdvanceSearch>().ReverseMap();
                config.CreateMap<DocumentTypeViewModel, DocumentType>().ReverseMap();
                config.CreateMap<DocumentApproverViewModel, DocumentApprover>().ReverseMap();
                config.CreateMap<CustomerCategoryViewModel, CustomerCategory>().ReverseMap();
                config.CreateMap<SaleOrderViewModel, SaleOrder>().ReverseMap();
                config.CreateMap<SaleOrderAdvanceSearchViewModel, SaleOrderAdvanceSearch>().ReverseMap();
                config.CreateMap<SaleOrderDetailViewModel, SaleOrderDetail>().ReverseMap();
                config.CreateMap<ProductionOrderViewModel, ProductionOrder>().ReverseMap();
                config.CreateMap<ProductionOrderAdvanceSearchViewModel, ProductionOrderAdvanceSearch>().ReverseMap();
                config.CreateMap<ProductionOrderDetailViewModel, ProductionOrderDetail>().ReverseMap();
                config.CreateMap<ServiceCallViewModel, ServiceCall>().ReverseMap();
                config.CreateMap<ServiceCallAdvanceSearchViewModel, ServiceCallAdvanceSearch>().ReverseMap();
                config.CreateMap<ServiceCallDetailViewModel, ServiceCallDetail>().ReverseMap();
                config.CreateMap<ProductionQCViewModel, ProductionQC>().ReverseMap();
                config.CreateMap<ProductionQCAdvanceSearchViewModel, ProductionQCAdvanceSearch>().ReverseMap();
                config.CreateMap<ProductionQCDetailViewModel, ProductionQCDetail>().ReverseMap();
            });
        }
    }
}
using AutoMapper;
using System.Collections.Generic;
using TechnipFMC.Finapp.Models;
using TechnipFMC.Finapp.Service.API.ViewModel;

namespace TechnipFMC.Finapp.Service.API.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<Country, CountryViewModel>().ReverseMap();
                config.CreateMap<ActionEntity, ActionViewModel>().ReverseMap();
                config.CreateMap<BillingType, BillingTypeViewModel>().ReverseMap();
                config.CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
                config.CreateMap<SubAction, SubActionViewModel>().ReverseMap();
                config.CreateMap<ProjectSegment, ProjectSegmentViewModel>().ReverseMap();
                config.CreateMap<ProjectEntity, ProjectEntityViewModel>().ReverseMap();
                config.CreateMap<BUCategory, BUCategoryViewModel>().ReverseMap();
                config.CreateMap<StatutoryCategory, StatutoryCategoryViewModel>().ReverseMap();
                config.CreateMap<ScenarioScopeTypes, ScenarioScopeTypesViewModel>().ReverseMap();
                config.CreateMap<FinancialDataTypeMapping, FinancialDataTypeMappingViewModel>().ReverseMap();
                config.CreateMap<FinancialDataTypes, FinancialDataTypesView>().ReverseMap();

                config.CreateMap<FinancialDataType, FinancialDataTypeViewModel>().ReverseMap();
                config.CreateMap<BillingType, BillingTypeViewModel>().ReverseMap();
                config.CreateMap<ContractType, ContractTypeViewModel>().ReverseMap();
                config.CreateMap<ContractStatus, ContractStatusViewModel>().ReverseMap();
                config.CreateMap<SmartViewCodeMaster, SmartViewCodeViewModel>().ReverseMap();
                config.CreateMap<GroupingParameters, GroupingParametersViewModel>().ReverseMap();
                config.CreateMap<ManagementCategory, ManagementCategoryViewModel>().ReverseMap();
                config.CreateMap<Project, ProjectViewModel>().ReverseMap();
                config.CreateMap<Projects, ProjectsViewModel>().ReverseMap();
                config.CreateMap<ScenarioScope, ScenarioScopeViewModel>().ReverseMap();
                config.CreateMap<ScenarioType, ScenarioTypeViewModel>().ReverseMap();
                config.CreateMap<Scenario, ScenarioViewModel>().ReverseMap();
                config.CreateMap<ProjectForScenario, ProjectForScenarioViewModel>().ReverseMap();
                config.CreateMap<ScenarioProject, ScenarioProjectViewModel>().ReverseMap();
                config.CreateMap<ScenarioApplicableYears, ScenarioApplicableYearsViewModel>().ReverseMap();
                config.CreateMap<ScenarioData, ScenarioDataViewModel>().ReverseMap();
                config.CreateMap<RolePermission, RolePermissionViewModel>().ReverseMap();
                config.CreateMap<UserPermission, UserPermissionViewModel>().ReverseMap();
                config.CreateMap<LockQuarter, LockQuarterViewModel>().ReverseMap();
                config.CreateMap<LockYear, LockYearViewModel>().ReverseMap();
                config.CreateMap<Currency, CurrencyViewModel>().ReverseMap();
                config.CreateMap<CurrencyExchange, CurrencyExchangeViewModel>().ReverseMap();
                config.CreateMap<ProjectsToLink, ProjectsToLinkViewModel>().ReverseMap();
                config.CreateMap<ScenarioUploadLog, ScenarioUploadLogViewModel>().ReverseMap();
                config.CreateMap<ProjectScenarioModel, ProjectScenarioViewModel>().ReverseMap();
                config.CreateMap<VarianceAnalysisConfig, VarianceAnalysisConfigViewModel>().ReverseMap();
                config.CreateMap<ScenarioProjectLogViewModel, ScenarioProjectLog>().ReverseMap();
                config.CreateMap<ScenarioIDS, ScenarioIDSViewModel>().ReverseMap();
                config.CreateMap<ScenarioID, ScenarioIDViewModel>().ReverseMap();
                config.CreateMap<ScenarioFile, ScenarioFileViewModel>().ReverseMap();
                config.CreateMap<ScenarioDataMaster, ScenarioDataMasterViewModel>().ReverseMap();
                config.CreateMap<ClubbingParameter, ClubbingParameterViewModel>().ReverseMap();
                config.CreateMap<VarianceAnalysisResponse, VarianceAnalysisResponseViewModel>().ReverseMap();
                config.CreateMap<VarianceAnalysisResponseGridModel, VarianceAnalysisResponseModel>().ReverseMap();
                config.CreateMap<ScenarioDDLModel, ScenarioDDLViewModel>().ReverseMap();
                config.CreateMap<ProjectLifeCycleData, ProjectLifeCycleViewModelData>().ReverseMap();
                config.CreateMap<ProjectLifeCycle, ProjectLifeCycleViewModel>().ReverseMap();
                config.CreateMap<RepExtractFullResponse, RepExtractFullResponseViewModel>().ReverseMap();
                config.CreateMap<CurrencyExchangeViewUIModel, CurrencyExchangeData>().ReverseMap();
                config.CreateMap<RepExtractViewModel, RepExtractFullResponseModel>().ReverseMap();
                config.CreateMap<ScenarioDetailsViewModel, ScenarioDetails>().ReverseMap();
                config.CreateMap<DetailedStructureViewModel, DetailedStructure>().ReverseMap();
                config.CreateMap<HeaderStructureViewModel, HeaderStructure>().ReverseMap();
                
                config.CreateMap<UserMaster, UserMasterViewModel>().ReverseMap();
                config.CreateMap<Client, ClientViewModel>().ReverseMap();
                config.CreateMap<Department, DepartmentViewModel>().ReverseMap();
                config.CreateMap<Customer, CustomerViewModel>().ReverseMap();

                config.CreateMap<ExtractDetailDataModel, ExtractDetailViewModel>().ReverseMap();
                config.CreateMap<ExtractHeaderDataModel, ExtractHeaderViewModel>().ReverseMap();
                config.CreateMap<ExtractResponseDataModel, ExtractResponseViewModel>().ReverseMap();
                config.CreateMap<ExtractDataDataModel, ExtractDataDataViewModel>().ReverseMap();
                config.CreateMap<ProjectLifeCycleDataModel, ProjectLifeCycleDataViewModel>().ReverseMap();
                config.CreateMap<DevianceReportConfig, DevianceReportConfigViewModel>().ReverseMap();
                config.CreateMap<DevianceResponse, DevianceResponseViewModel>().ReverseMap();
                config.CreateMap<DepartmentData, DepartmentDataViewModel>().ReverseMap();
                config.CreateMap<ClientData, ClientDataViewModel>().ReverseMap();
                config.CreateMap<DevianceGridResponse, DevianceGridResponseViewModel>().ReverseMap();
                config.CreateMap<DeviationAnalysisYearWiseModel, DeviationAnalysisYearWiseViewModel>().ReverseMap();
                config.CreateMap<DashboardDataModel, DashboardDataViewModel>().ReverseMap();
                config.CreateMap<NonOrgDataModel, NonOrgDataViewModel>().ReverseMap();
                config.CreateMap<ProjectDataModel, ProjectDataViewModel>().ReverseMap();
                config.CreateMap<DepartmentWiseDataModel, DepartmentWiseDataViewModel>().ReverseMap();
                config.CreateMap<BudgetDeviationDataModel, BudgetDeviationDataViewModel>().ReverseMap();
                config.CreateMap<FinancialDataGross, FinancialDataGrossViewModel>().ReverseMap();
                config.CreateMap<DifferenceData, DifferenceDataViewModel>().ReverseMap();
                config.CreateMap<DashboardConfig, DashboardConfigViewModel>().ReverseMap();
                config.CreateMap<ChangePasswordModel, ChangePasswordViewModel>().ReverseMap();
                config.CreateMap<ForgotPasswordModel, ForgotPasswordViewModel>().ReverseMap();
                config.CreateMap<FinancePerformanceDataModel, FinancePerformanceDataViewModel>().ReverseMap();
                config.CreateMap<ProjectPerformanceDataModel, ProjectPerformanceDataViewModel>().ReverseMap();
                config.CreateMap<ProjectGross, ProjectGrossViewModel>().ReverseMap();
                config.CreateMap<TrendReportData, TrendReportDataViewModel>().ReverseMap();
                config.CreateMap<TrendDataModel, TrendDataViewModel>().ReverseMap();
                config.CreateMap<SignUpCountry, SignUpCountryViewModel>().ReverseMap();
                config.CreateMap<PlanDetails, PlanDetailsViewModel>().ReverseMap();




            });
        }
    }
}
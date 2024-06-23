using System;
using TechnipFMC.Finapp.Business;
using Unity;
using TechnipFMC.Finapp.Business.Interfaces;
namespace TechnipFMC.Finapp.Service.API
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // Register your type's mappings here.
            container.RegisterType<ICountryBL, CountryBL>();
            container.RegisterType<IActionBL, ActionBL>();
            container.RegisterType<IBillingTypeBL, BillingTypeBL>();
            container.RegisterType<ISubActionBL, SubActionBL>();
            container.RegisterType<IUserRoleBL, UserRoleBL>();
            container.RegisterType<IProjectSegmentBL,ProjectSegmentBL>();
            container.RegisterType<IProjectEntityBL, ProjectEntityBL>();
            container.RegisterType<IBUCategoryBL,BUCategoryBL>();
            container.RegisterType<IStatutoryCategoryBL,StatutoryCategoryBL>();
            container.RegisterType<IContractTypeBL,ContractTypeBL>();
            container.RegisterType<IContractStatusBL,ContractStatusBL>();
            container.RegisterType<ISmartViewCodeBL,SmartViewCodeBL>();
            container.RegisterType<IGroupingParametersBL, GroupingParametersBL>();
            container.RegisterType<IManagementCategoryBL,ManagementCategoryBL>();
            container.RegisterType<IProjectBL, ProjectBL>();
            container.RegisterType<IScenarioScopeBL, ScenarioScopeBL>();
            container.RegisterType<IScenarioTypeBL, ScenarioTypeBL>();
            container.RegisterType<IScenarioBL, ScenarioBL>();
            container.RegisterType<IScenarioDataBL, ScenarioDataBL>();
            container.RegisterType<IRolePermissionBL, RolePermissionBL>();
            container.RegisterType<ILockQuarterBL, LockQuarterBL>();
            container.RegisterType<IScenarioFileBL, ScenarioFileBL>();
            container.RegisterType<IUserPermissionBL, UserPermissionBL>();
            container.RegisterType<ILockYearBL, LockYearBL>();
            container.RegisterType<ICurrencyBL, CurrencyBL>();
            container.RegisterType<ICurrencyExchangeBL, CurrencyExchangeBL>();
            container.RegisterType<IReportBL, ReportBL>();
            container.RegisterType<IClubbingParameterBL, ClubbingParameterBL>();
            container.RegisterType<IClientBL, ClientBL>();
            container.RegisterType<IUserMasterBL, UserMasterBL>();
            container.RegisterType<IDepartmentBL, DepartmentBL>();
            container.RegisterType<ICustomerBL, CustomerBL>();
            container.RegisterType<IFinancialDataTypeBL, FinancialDataTypeBL>();
            container.RegisterType<IFinancialDataTypeScenarioBL, FinancialDataTypeScenarioBL>();
           


            //container.AddNewExtension<DependencyInjectionExtension>();
        }
    }
}
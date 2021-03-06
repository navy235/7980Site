[assembly: WebActivator.PreApplicationStartMethod(typeof(PadSite.NinjectBinding), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(PadSite.NinjectBinding), "Stop")]

namespace PadSite
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Maitonn.Core;
    using PadSite.Models;
    using PadSite.Service.Interface;
    using PadSite.Service;

    public static class NinjectBinding
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<EntitiesContext>().InRequestScope();

            #region category
            kernel.Bind<ICityCateService>().To<CityCateService>().InRequestScope();
            kernel.Bind<IMediaCateService>().To<MediaCateService>().InRequestScope();
            kernel.Bind<IArticleCateService>().To<ArticleCateService>().InRequestScope();
            kernel.Bind<IFormatCateService>().To<FormatCateService>().InRequestScope();
            kernel.Bind<IPeriodCateService>().To<PeriodCateService>().InRequestScope();

            kernel.Bind<IIndustryCateService>().To<IndustryCateService>().InRequestScope();
            kernel.Bind<ICrowdCateService>().To<CrowdCateService>().InRequestScope();
            kernel.Bind<IOwnerCateService>().To<OwnerCateService>().InRequestScope();
            kernel.Bind<IAreaCateService>().To<AreaCateService>().InRequestScope();
            kernel.Bind<IPurposeCateService>().To<PurposeCateService>().InRequestScope();
            #endregion

            #region permission
            kernel.Bind<IDepartmentService>().To<DepartmentService>().InRequestScope();
            kernel.Bind<IPermissionsService>().To<PermissionsService>().InRequestScope();
            kernel.Bind<IRolesService>().To<RolesService>().InRequestScope();
            kernel.Bind<IGroupService>().To<GroupService>().InRequestScope();
            #endregion


            #region member
            kernel.Bind<IMemberService>().To<MemberService>().InRequestScope();
            kernel.Bind<IMember_ActionService>().To<Member_ActionService>().InRequestScope();
            kernel.Bind<ICompanyService>().To<CompanyService>().InRequestScope();
            kernel.Bind<ICompanyCredentialsImgService>().To<CompanyCredentialsImgService>().InRequestScope();
            kernel.Bind<ICompanyNoticeService>().To<CompanyNoticeService>().InRequestScope();
            kernel.Bind<ICompanyMessageService>().To<CompanyMessageService>().InRequestScope();
            #endregion

            #region Message
            kernel.Bind<IMessageService>().To<MessageService>().InRequestScope();
            #endregion

            #region Lucene
            kernel.Bind<IOutDoorLuceneService>().To<OutDoorLuceneService>().InRequestScope();
            #endregion

            #region Media
            kernel.Bind<IOutDoorService>().To<OutDoorService>().InRequestScope();
            kernel.Bind<IFavoriteService>().To<FavoriteService>().InRequestScope();
            kernel.Bind<ISchemeService>().To<SchemeService>().InRequestScope();
            kernel.Bind<ISchemeItemService>().To<SchemeItemService>().InRequestScope();
            #endregion

            #region email
            kernel.Bind<IEmailService>().To<EmailService>().InRequestScope();
            #endregion

            #region article
            kernel.Bind<IArticleService>().To<ArticleService>().InRequestScope();
            #endregion

        }
    }
}

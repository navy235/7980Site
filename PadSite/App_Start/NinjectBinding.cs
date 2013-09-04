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
            kernel.Bind<ICityCateService>().To<CityCateService>().InRequestScope();
            kernel.Bind<IMediaCateService>().To<MediaCateService>().InRequestScope();
            kernel.Bind<IArticleCateService>().To<ArticleCateService>().InRequestScope();
        }
    }
}

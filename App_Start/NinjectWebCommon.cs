[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MyWebApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MyWebApp.App_Start.NinjectWebCommon), "Stop")]

namespace MyWebApp.App_Start
{
    using System;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using MyWebApp.Repositories.Interfaces;
    using MyWebApp.Repositories;
    using MyWebApp.Models;

    public static class NinjectWebCommon 
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
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
               

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<IProblemRepository>().To<ProblemRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<IAnswerRepository>().To<AnswerRepository>();
            kernel.Bind<IUserSolvedRepository>().To<UserSolvedRepository>();
            kernel.Bind<IUserAttemptedRepository>().To<UserAttemptedRepository>();
          
            kernel.Bind<IImageRepository>().To<ImageRepository>();
            kernel.Bind<ITagRepository>().To<TagRepository>();
            kernel.Bind<ILikeRepository>().To<LikeRepository>();
            kernel.Bind<IDislikeRepository>().To<DislikeRepository>();
            kernel.Bind<ICommentRepository>().To<CommentRepository>();
        }        
    }
}

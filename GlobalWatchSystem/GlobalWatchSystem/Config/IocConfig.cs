using System.Reflection;
using System.Web.Mvc;
using Ninject;
using SharpRepository.Ioc.Ninject;
using SharpRepository.Repository.Ioc;
using NinjectDependencyResolver = GlobalWatchSystem.Ioc.NinjectDependencyResolver;

namespace GlobalWatchSystem
{
    public static class IocConfig
    {
        public static void Register()
        {
            IKernel kernel = CreateNinjectKernel();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            RepositoryDependencyResolver.SetDependencyResolver(
                new SharpRepository.Ioc.Ninject.NinjectDependencyResolver(kernel));
        }

        private static IKernel CreateNinjectKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.BindSharpRepository();
            return kernel;
        }
    }
}
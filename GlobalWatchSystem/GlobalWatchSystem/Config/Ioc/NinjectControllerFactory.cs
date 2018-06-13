using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace GlobalWatchSystem.Ioc
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel ninjectKernel;

        public NinjectControllerFactory(IKernel ninjectKernel)
        {
            this.ninjectKernel = ninjectKernel;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) ninjectKernel.Get(controllerType);
        }
    }
}
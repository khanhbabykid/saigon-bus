using System;
using Ninject;
using System.Web.Mvc;
using Ninject.Modules;
using System.Data.Entity;
using ITS.Business.Concrete;
using ITS.Business.Abstract;
using ITS.DAL.Repository;
using ITS.DAL.Implementation.Repository;
namespace ITS.Admin
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel(new BindingServicesModule());
        }
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext,
            Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

    }

    public static class NinjectFactory
    {
        private static IKernel kernel;
        public static IKernel Kernel
        {
            get
            {
                if (kernel == null)
                {
                    kernel = new StandardKernel(new BindingServicesModule());
                }
                return kernel;
            }
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }


    public class BindingServicesModule : NinjectModule
    {
        public override void Load()
        {
            /*Business*/
            Bind<IBusService>().To<BusService>();

            /*Repository*/
            Bind<IBusRepository>().To<BusRepository>();
        }
    }
}

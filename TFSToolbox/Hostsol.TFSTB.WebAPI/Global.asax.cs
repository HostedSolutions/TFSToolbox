using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web;

namespace Hostsol.TFSTB.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication , IContainerProviderAccessor
    {
        static IContainer _container;
        static IContainerProvider _containerProvider;
        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var containerBuilder = new ContainerBuilder();
            var asm = Assembly.GetExecutingAssembly();

            containerBuilder.RegisterAssemblyTypes(asm)
                       .Where(t => t.Name.EndsWith("Access"))
                       .AsImplementedInterfaces();
            
            containerBuilder.RegisterModule<Settings.ConfigurationModule>();

            _container = containerBuilder.Build();
            

            _containerProvider = new ContainerProvider(_container);
        }
    }
}

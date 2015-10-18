using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web;

namespace Hostsol.TFSTB.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
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

            containerBuilder.RegisterType<TFS.ServerAccess>();
            containerBuilder.RegisterType<TFS.BuildAccess>();
            containerBuilder.RegisterType<TFS.VersionControlAccess>();
            containerBuilder.RegisterType<TeamCity.BuildAccess>();

            _container = containerBuilder.Build();

            _container.Resolve<TeamCity.BuildAccess>();
            _container.Resolve<TFS.ServerAccess>();
            _container.Resolve<TFS.BuildAccess>();
            _container.Resolve<TFS.VersionControlAccess>();

            _containerProvider = new ContainerProvider(_container);
        }
    }
}

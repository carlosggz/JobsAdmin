using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using JobsAdmin.Core.Contracts;
using JobsAdmin.Handler;
using JobsAdmin.Jobs;
using JobsAdmin.Web.Hub;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JobsAdmin.Web.Startup))]
namespace JobsAdmin.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var handler = JobsHandler.Instance;

            var builder = new ContainerBuilder();
            builder.RegisterHubs(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterInstance(handler).As<IJobsHandler>();

            var container = builder.Build();

            var signalResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
            GlobalHost.DependencyResolver = signalResolver;
            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);

            app.MapSignalR("/signalr", new HubConfiguration()
            {
                Resolver = signalResolver
            });

            handler.Hosting = new JobScheduler();
            handler.Notifier = new JobsHandlerNotifier(GlobalHost.ConnectionManager);
        }
    }
}
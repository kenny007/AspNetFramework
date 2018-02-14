using OONLFWK.Infrastructure;
using OONLFWK.Infrastructure.ModelMetadata;
using OONLFWK.Infrastructure.Tasks;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OONLFWK
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //This is a wrapper property to simplify storing and retrieving of container and store in the Item Collection of the
        //current HttpContext since that collection is already tied to the currently executing WebRequest
        public IContainer Container
        {
            get
            {
                return (IContainer)HttpContext.Current.Items["_Container"];
            }
            set
            {
                HttpContext.Current.Items["_Container"] = value;
            }
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Instructing MVC to use our Dependency Resolver
            //So at runtime it will use our nested Container it available or else it just falls back and use ObjectFactory 
            //container instead so the fallback is necessary
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(() => Container ?? ObjectFactory.Container));
             

            //This scans the types in our application and applies the default conventions
          ObjectFactory.Configure(cfg =>
            {
               //Registeries are structuremap concepts that helps us modularize our configurations into separate classes
                cfg.AddRegistry(new StandardRegistry());
                cfg.AddRegistry(new ControllerRegistry());
                //At runtime MVC will ask structuremap if it has an implementation of IFilterProvider
                cfg.AddRegistry(new ActionFilterRegistry(
                    () => Container ?? ObjectFactory.Container));
                cfg.AddRegistry(new MvcRegistry());
                cfg.AddRegistry(new TaskRegistry());
                cfg.AddRegistry(new ModelMetadataRegistry());
            });

          using (var container = ObjectFactory.Container.GetNestedContainer())
          {
              foreach (var task in container.GetAllInstances<IRunAtInit>())
              {
                  task.Execute();
              }

              foreach (var task in container.GetAllInstances<IRunAtStartup>())
              {
                  task.Execute();
              }
          }

        }

        //At the beginning of each request we create a nested container from ObjectFactory static container
        public void Application_BeginRequest()
        {
            Container = ObjectFactory.Container.GetNestedContainer();

            foreach (var task in Container.GetAllInstances<IRunOnEachRequest>())
            {
                task.Execute();
            }
        }

        public void Application_Error()
        {
            foreach (var task in Container.GetAllInstances<IRunOnError>())
            {
                task.Execute();
            }
        }

        public void Application_EndRequest()
        {

            try
            {
                foreach (var task in
                    Container.GetAllInstances<IRunAfterEachRequest>())
                {
                    task.Execute();
                }
            }
            finally
            {
                Container.Dispose();
                Container = null;
            }
        }
    }
}

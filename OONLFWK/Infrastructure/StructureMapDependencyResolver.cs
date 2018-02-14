using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace OONLFWK.Infrastructure
{
    public class StructureMapDependencyResolver:IDependencyResolver
    {
        private readonly Func<IContainer> _containerFactory;
        public StructureMapDependencyResolver(Func<IContainer> containerFactory)
        {
            _containerFactory = containerFactory;
        }
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            var container = _containerFactory();
          //  var factory = _factory();
            //use the above function to obtain reference to these methods from each of our containers
            return serviceType.IsAbstract || serviceType.IsInterface
                ? container.TryGetInstance(serviceType)
                : container.GetInstance(serviceType);
        }
        //Gets all services of a particular type
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _containerFactory().GetAllInstances(serviceType).Cast<object>();
        }
    }
}
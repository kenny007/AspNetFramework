using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.TypeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OONLFWK.Infrastructure
{
    public class ActionFilterRegistry : Registry
    {
        public ActionFilterRegistry(Func<IContainer> containerFactory)
        {
            //This states we're going to register our StructureMapFilter Provider
            For<IFilterProvider>().Use(
                new StructureMapFilterProvider(containerFactory));

            //Convention for how structuremap should perform setter injection into our ActionFilter
            SetAllProperties(x =>
                x.Matching(p =>
                    p.DeclaringType.CanBeCastTo(typeof(ActionFilterAttribute)) &&
                    p.DeclaringType.Namespace.StartsWith("Wareman") &&
                    !p.PropertyType.IsPrimitive &&
                    p.PropertyType != typeof(string)));
        }
    }
}
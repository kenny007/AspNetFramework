using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OONLFWK.Infrastructure
{
    public class ControllerConvention:IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            //This will cause StructureMap to create a new type even though it is called from a nested container
            if (type.CanBeCastTo(typeof(Controller)) && !type.IsAbstract)
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }
        }
    }
}
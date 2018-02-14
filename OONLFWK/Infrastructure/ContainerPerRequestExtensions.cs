using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OONLFWK.Infrastructure
{
    public static class ContainerPerRequestExtensions
    {
        public static IContainer GetContainer(this HttpContextBase context)
        {
            return (IContainer)HttpContext.Current.Items["_Container"]
                ?? ObjectFactory.Container;
        }
    }
}
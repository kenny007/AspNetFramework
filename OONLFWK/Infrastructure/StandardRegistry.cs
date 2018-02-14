using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OONLFWK.Infrastructure
{
    public class StandardRegistry:Registry
    {
        //We use Fluent API to scan the calling assembly
        public StandardRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OONLFWK.Startup))]
namespace OONLFWK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

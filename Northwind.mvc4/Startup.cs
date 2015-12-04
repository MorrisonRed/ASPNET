using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPNET.Startup))]
namespace ASPNET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

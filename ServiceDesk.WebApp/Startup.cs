using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServiceDesk.WebApp.Startup))]
namespace ServiceDesk.WebApp
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}

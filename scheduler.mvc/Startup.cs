using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(scheduler.mvc.Startup))]
namespace scheduler.mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

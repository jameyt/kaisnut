using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(draftday.mvc.Startup))]
namespace draftday.mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

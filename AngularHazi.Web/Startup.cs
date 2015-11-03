using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AngularHazi.Web.Startup))]
namespace AngularHazi.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

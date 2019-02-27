using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(darkZencefil.Startup))]
namespace darkZencefil
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageSharingWithCloudService.Startup))]
namespace ImageSharingWithCloudService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

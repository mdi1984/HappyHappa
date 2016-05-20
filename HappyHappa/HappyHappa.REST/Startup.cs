using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using HappyHappa.REST.App_Start;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;

namespace HappyHappa.REST
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      HttpConfiguration config = new HttpConfiguration();

      app.UseNinjectMiddleware(() => NinjectConfig.CreateKernel.Value);
      app.UseNinjectWebApi(config);

      app.UseFileServer(new FileServerOptions
      {
        RequestPath = new PathString(string.Empty),
        FileSystem = new PhysicalFileSystem("./wwwroot"),
        EnableDirectoryBrowsing = false
      });

      WebApiConfig.Register(config);
    }
  }
}
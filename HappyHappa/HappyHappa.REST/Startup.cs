using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using HappyHappa.REST.App_Start;

namespace HappyHappa.REST
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      HttpConfiguration config = new HttpConfiguration();

      app.UseNinjectMiddleware(() => NinjectConfig.CreateKernel.Value);
      app.UseNinjectWebApi(config);

      WebApiConfig.Register(config);
    }
  }
}
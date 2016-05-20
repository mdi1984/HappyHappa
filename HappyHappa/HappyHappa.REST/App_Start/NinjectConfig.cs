using System;
using System.Reflection;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using HappyHappa.Data.MongoDb.Data;
using HappyHappa.Data.MongoDb;
using HappyHappa.REST.DAL;

namespace HappyHappa.REST.App_Start
{
  public static class NinjectConfig
  {
    public static Lazy<IKernel> CreateKernel = new Lazy<IKernel>(() =>
    {
      var kernel = new StandardKernel();
      kernel.Load(Assembly.GetExecutingAssembly());

      RegisterServices(kernel);
      return kernel;
    });

    private static void RegisterServices(IBindingRoot kernel)
    {
      kernel.Bind<IRepository>().To<Repository>().InRequestScope();
      kernel.Bind<IDAL>().To<DAL.DAL>().InRequestScope();
    }
  }
}
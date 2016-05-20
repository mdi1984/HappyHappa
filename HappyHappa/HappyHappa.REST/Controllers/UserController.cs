using HappyHappa.REST.DAL;
using HappyHappa.REST.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HappyHappa.REST.Controllers
{
  public class UserController : AbstractController
  {
    public UserController(IDAL dal): base(dal) { }

    public async Task<User> Get()
    {
      return await Dal.GetUser();
    }

    public async Task<User> Put(User u)
    {
      return await Dal.SetUser(u);
    }
  }
}
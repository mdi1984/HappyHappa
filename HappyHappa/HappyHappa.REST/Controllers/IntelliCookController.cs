using HappyHappa.REST.DAL;
using HappyHappa.REST.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HappyHappa.REST.Controllers
{
  public class IntelliCookController : AbstractController
  {
    public IntelliCookController(IDAL dal): base(dal) { }
    public async Task<IEnumerable<Recipe>> Get(string id)
    {
      return await Dal.GetMostMatchingRecipes(id);
    }
  }
}
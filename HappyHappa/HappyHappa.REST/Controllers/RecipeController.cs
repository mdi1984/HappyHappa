using HappyHappa.REST.DAL;
using HappyHappa.REST.DAL.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HappyHappa.REST.Controllers
{
  public class RecipeController : AbstractController
  {
    public RecipeController(IDAL dal): base(dal) { }

    public async Task<IEnumerable<Recipe>> Get()
    {
      return await Dal.GetRecipes();
    }

    public async Task<Recipe> Get(string id)
    {
      return await Dal.GetRecipe(id);
    }

    public async Task<Recipe> Push([FromBody] Recipe recipe)
    {
      return await Dal.SaveRecipe(recipe);
    }

  }
}
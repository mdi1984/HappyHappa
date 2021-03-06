﻿using HappyHappa.REST.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyHappa.REST.DAL
{
  public interface IDAL
  {
    Task<Model.Item> PutItem(BoughtItem item);
    Task<Model.Item> TakeItem(BoughtItem item);
    Task<IEnumerable<Model.Item>> TakeItems(IEnumerable<BoughtItem> items);

    Task<string> CreateFridge(Device device);

    Task<User> GetUser();
    Task<User> SetUser(User user);

    Task<IEnumerable<Item>> GetItems(string fridgeId, bool abs = false);

    Task<Recipe> GetRecipe(string recipeId);
    Task<IEnumerable<Recipe>> GetRecipes();
    Task<Recipe> SaveRecipe(Recipe recipe);

    Task<IEnumerable<Recipe>> GetMostMatchingRecipes(string fridgeId);
  }
}

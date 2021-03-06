﻿using HappyHappa.Data.MongoDb.Data;
using HappyHappa.REST.DAL.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace HappyHappa.REST.DAL
{
  public class DAL : IDAL
  {
    private static object synchronizationToken;
    private IContext ctx;
    public IRepository repo { get; set; }

    public DAL(IRepository repository)
    {
      repo = repository;
    }

    public async Task<Item> PutItem(BoughtItem newItem)
    {
      Product product = new Product
      {
        Amount = newItem.Amount,
        ExpirationDate = newItem.ExpirationDate,
      };

      Fridge fridge = await RetrieveFridge(newItem.FridgeId);
      Model.Item item = fridge.Items.FirstOrDefault(val => val.Name == newItem.ItemName);

      if (item == null)
      {
        item = new Model.Item
        {
          Name = newItem.ItemName,
          Products = new List<Product>()
        };
        (fridge.Items as IList<Item>).Add(item);
      }
      (item.Products as IList<Product>).Add(product);

      var updateRequest = await repo.UpdateAsync(fridge);

      if (!updateRequest.Success) throw new Exception($"{item.Name} coulnd't be updated");

      return updateRequest.Success ? item : null;
    }

    public async Task<IEnumerable<Item>> TakeItems(IEnumerable<BoughtItem> items)
    {
      Fridge fridge = await RetrieveFridge(items.First().FridgeId);
      foreach(BoughtItem item in items)
      {
        Model.Item persistedItem = fridge.Items.FirstOrDefault(val => val.Name.ToLower() == item.ItemName.ToLower());

        //item not present.
        if (persistedItem == null) throw new Exception($"There is no {item.ItemName} in your fridge");

        //not enough items present
        if (item.Amount > persistedItem.GetTotalAmount()) throw new Exception($"Not enough {item.ItemName} present");

        int neededItems = item.Amount;
        //take items
        var products = persistedItem.Products.OrderBy(product => product.ExpirationDate).ThenBy(product => product.ExpirationDate == null);
        foreach (Product product in products.TakeWhile(product => neededItems >= 0))
        {
          int residue = product.Amount - neededItems;
          //delete entry
          if (residue <= 0)
          {
            (persistedItem.Products as IList<Product>).Remove(product);
            neededItems = Math.Abs(residue);
          }
          //change entry
          else
          {
            product.Amount = residue;
            neededItems = 0;
          }
        }

        if ((persistedItem.Products as IList<Product>).Count == 0)
        {
          (fridge.Items as IList<Item>).Remove(persistedItem);
        }
      }

      var updateRequest = await repo.UpdateAsync(fridge);

      return updateRequest.Success ? await GetItems(fridge.Id) : null;
    }

    public async Task<Item> TakeItem(BoughtItem item)
    {
      Fridge fridge = await RetrieveFridge(item.FridgeId);
      Model.Item persistedItem = fridge.Items.FirstOrDefault(val => val.Name.ToLower() == item.ItemName.ToLower());

      //item not present.
      if (persistedItem == null) throw new Exception($"There is no {item.ItemName} in your fridge");

      //not enough items present
      if (item.Amount > persistedItem.GetTotalAmount()) throw new Exception($"Not enough {item.ItemName} present");

      int neededItems = item.Amount;
      //take items
      var products = persistedItem.Products.OrderBy(product => product.ExpirationDate).ThenBy(product => product.ExpirationDate == null);
      foreach (Product product in products.TakeWhile(product => neededItems >= 0))
      {
        int residue = product.Amount - neededItems;
        //delete entry
        if (residue <= 0)
        {
          (persistedItem.Products as IList<Product>).Remove(product);
          neededItems = Math.Abs(residue);
        }
        //change entry
        else
        {
          product.Amount = residue;
          neededItems = 0;
        }
      }

      if((persistedItem.Products as IList<Product>).Count == 0)
      {
        (fridge.Items as IList<Item>).Remove(persistedItem);
      }

      var updateRequest = await repo.UpdateAsync(fridge);

      if (!updateRequest.Success) throw new Exception($"{persistedItem.Name} coulnd't be updated");

      return updateRequest.Success ? persistedItem : null;
    }

    private async Task<Fridge> RetrieveFridge(string fridgeId)
    {
      var fridgeRequest = await repo.SingleAsync<Fridge>(fr => fr.Id == fridgeId);
      if (!fridgeRequest.Success) throw new Exception($"Fridge with Id {fridgeId} not found");

      return fridgeRequest.Data;
    }

    public async Task<string> CreateFridge(Device device)
    {
      var response = await repo.SingleAsync<Fridge>(f => f.DeviceId == device.DeviceAddress);
      if (response.Success) return response.Data.Id;

      Fridge fridge = new Fridge
      {
        DeviceId = device.DeviceAddress,
        Items = new List<Model.Item>()
      };

      var fridgeResponse = await repo.AddAsync(fridge);
      if (!fridgeResponse.Success) throw new Exception("Fridge registration unsuccessful");
      return fridgeResponse.Data.Id;
    }

    public async Task<User> GetUser()
    {
      var response = await repo.SingleAsync<User>(user => true);
      if (!response.Success && response.Errors.Any(error => error.Equals("no items found")))
      {
        await repo.AddAsync(new User
        {
          FridgeSecret = null
        });
        response = await repo.SingleAsync<User>(user => true);
      }
      return response.Data;
    }

    public async Task<User> SetUser(User user)
    {
      User u = await GetUser();
      u.FridgeSecret = user.FridgeSecret;

      var response = await repo.UpdateAsync(u);
      if (!response.Success) throw new Exception("Updating User Setting failed");

      return u;
    }

    public async Task<IEnumerable<Item>> GetItems(string fridgeId, bool abs = false)
    {
      Fridge fridge = await RetrieveFridge(fridgeId);
      if (abs)
      {
        fridge.Items = fridge.Items.Select(item => new Item
        {
          Name = item.Name,
          Products = new List<Product>
          {
            new Product
            {
              Amount = item.Products.Sum(product => product.Amount),
              ExpirationDate = null
            }
          }
        });
      }

      return fridge.Items;
    }

    public async Task<Recipe> GetRecipe(string recipeId)
    {
      Recipe recipe = (await GetRecipes()).FirstOrDefault(r => r.Id.Equals(recipeId));
      if (recipe == null) throw new Exception("Recipe not found");

      return recipe;
    }

    public async Task<IEnumerable<Recipe>> GetRecipes()
    {
      var recipeRequest = await repo.AllAsync<Recipe>();
      if (!recipeRequest.Success) throw new Exception("Recipes couldn't be retrieved");

      return recipeRequest.Data;
    }

    public async Task<Recipe> SaveRecipe(Recipe recipe)
    {
      var addResponse = await repo.AddAsync(recipe);
      if (!addResponse.Success) throw new Exception("Recipe couldn't be saved");

      return addResponse.Data;
    }

    public async Task<IEnumerable<Recipe>> GetMostMatchingRecipes(string fridgeId)
    {
      var items = await GetItems(fridgeId);
      var recipes = await GetRecipes();

      var test = recipes.Select(recipe => new EvaluatedRecipe { Recipe = recipe, Evaluation = evaluateRecipe(recipe, items) }).OrderByDescending(recipe => recipe.Evaluation);
      return test
                    .Take(5)
                    .Select(recipe => recipe.Recipe);
    }

    private double evaluateRecipe(Recipe recipe, IEnumerable<Item> items)
    {
      double weightExpiration = 0.73;
      double weightPresentItems = 0.20;
      double weightRating = 0.07;

      IList<Ingredient> ingredients = recipe.Ingredients as IList<Ingredient>;
      int maxIngredients = ingredients.Count;
      int presentIngredients = 0;
      foreach(Ingredient ingredient in ingredients)
      {
        var presentItem = items.FirstOrDefault(item => item.Name == ingredient.Name);
        if (presentItem != null && presentItem.GetTotalAmount() >= ingredient.Amount) presentIngredients++;
      }
      double presentIngredientsQuota = maxIngredients == 0 ? 0: presentIngredients / (double) maxIngredients;

      long minExpiration = DateTime.MinValue.Ticks;
      long maxExpiration = DateTime.MaxValue.Ticks;
      minExpiration = items.Select(item => item.Products.Where(product => product.ExpirationDate != null).Min(product => product.ExpirationDate)).Min().Value.Ticks;
      maxExpiration = items.Select(item => item.Products.Where(product => product.ExpirationDate != null).Max(product => product.ExpirationDate)).Max().Value.Ticks;
      var minExpirations = new List<long>();
      try {
        minExpirations = items.Where(item => ingredients.Any(ing => ing.Name == item.Name))
                                .Select(item => item.Products.Where(product => product.ExpirationDate != null).Min(product => product.ExpirationDate).Value.Ticks).ToList();
      }catch(Exception){ }
      double expirationQuota = 1;
      if (minExpirations.Any())
      {
        foreach(long expirationValue in minExpirations)
        {
          expirationQuota *= (int) ((expirationValue - maxExpiration) * 100 / (minExpiration - maxExpiration) / 100);
        }
      }
      else
      {
        expirationQuota = 0;
      }
      double result = weightPresentItems * presentIngredientsQuota + expirationQuota * weightExpiration + weightRating * recipe.Rating / 5;
      return result;
    }
  }
}
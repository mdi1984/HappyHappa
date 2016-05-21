using HappyHappa.Data.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Model
{
  public class Recipe : Entity
  {
    public string Name { get; set; }
    public IEnumerable<Ingredient> Ingredients { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Entity
{
  public class Recipe
  {
    public string Name { get; set; }
    public IEnumerable<Item> Ingredients { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Entity
{
  public class Fridge
  {
    public IEnumerable<Item> Items { get; set; }
  }
}
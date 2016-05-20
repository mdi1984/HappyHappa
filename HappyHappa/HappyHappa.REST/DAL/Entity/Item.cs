using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Entity
{
  public class Item
  {
    public string Name { get; set; }
    public IEnumerable<Product> Products { get; set; }
  }
}
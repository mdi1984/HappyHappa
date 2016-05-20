using HappyHappa.Data.MongoDb;
using System.Collections.Generic;
using System.Linq;

namespace HappyHappa.REST.DAL.Model
{
  public class Item
  {
    public string Name { get; set; }
    public IEnumerable<Product> Products { get; set; }

    public int GetTotalAmount()
    {
      return Products.Sum(product => product.Amount);
    }
  }
}
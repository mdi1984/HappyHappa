using HappyHappa.Data.MongoDb;
using HappyHappa.Data.MongoDb.Data;
using System.Collections.Generic;

namespace HappyHappa.REST.DAL.Model
{
  [CollectionName("Fridge")]
  public class Fridge : Entity
  {
    public IEnumerable<Item> Items { get; set; }
    public string DeviceId { get; set; }
  }
}
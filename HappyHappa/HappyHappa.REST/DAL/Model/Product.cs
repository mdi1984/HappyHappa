using HappyHappa.Data.MongoDb;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace HappyHappa.REST.DAL.Model
{
  public class Product
  {
    public int Amount { get; set; }

    public DateTime? ExpirationDate { get; set; }
  }
}
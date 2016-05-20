using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Entity
{
  public class Product
  {
    public int Amount { get; set; }
    public DateTime ExpirationDate { get; set; }
  }
}
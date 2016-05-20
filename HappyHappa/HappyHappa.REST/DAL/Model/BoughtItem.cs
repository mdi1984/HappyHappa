using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Model
{
  public class BoughtItem
  {
    public string FridgeId { get; set; }
    public string ItemName { get; set; }
    public int Amount { get; set; }
    public DateTime? ExpirationDate { get; set; }
  }
}
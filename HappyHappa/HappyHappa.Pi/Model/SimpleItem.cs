﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyHappa.Pi.Model
{
  public class SimpleItem
  {
    public string FridgeId { get; set; }
    public string ItemName { get; set; }
    public int Amount { get; set; }
    public DateTime? ExpirationDate { get; set; }
  }
}

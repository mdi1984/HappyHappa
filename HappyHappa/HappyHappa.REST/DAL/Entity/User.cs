using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Entity
{
  public class User
  {
    public int Id { get; set; }
    public Fridge Fridge { get; set; }
  }
}
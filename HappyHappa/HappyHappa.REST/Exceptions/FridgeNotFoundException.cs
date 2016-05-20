using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.Exceptions
{
  public class FridgeNotFoundException : Exception
  {
    public FridgeNotFoundException(string msg): base(msg) { }
  }
}
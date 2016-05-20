using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HappyHappa.REST.DAL.Model;

namespace HappyHappa.REST.DAL
{
  public class DummyContext : IContext
  {
    private IList<Item> items;

    public IList<Item> Items
    {
      get
      {
        return items;
      }

      set
      {
        throw new Exception("Not allowed");
      }
    }

    public void Save() { }
  }
}
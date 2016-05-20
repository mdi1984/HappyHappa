using HappyHappa.REST.DAL.Model;
using System.Collections.Generic;

namespace HappyHappa.REST.DAL
{
  public interface IContext
  {
    IList<Item> Items { get; set; }
    void Save();
  }
}

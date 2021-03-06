﻿using HappyHappa.REST.DAL;
using HappyHappa.REST.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace HappyHappa.REST.Controllers
{
  public class ItemController : AbstractController
  {
    public ItemController(IDAL dal) : base(dal) { }

    public async Task<IEnumerable<Item>> Get(string id)
    {
      return await Dal.GetItems(id);
    }

    public async Task<IEnumerable<Item>> Get(string id, [FromUri] bool abs)
    {
      return await Dal.GetItems(id, abs);
    }

    // PUT api/<controller>
    public async Task<Item> Put([FromBody] BoughtItem item)
    {
      return await Dal.PutItem(item);
    }

    // DELETE api/<controller>
    public async Task<IEnumerable<Item>> Delete([FromBody] IEnumerable<BoughtItem> item)
    {
      return await Dal.TakeItems(item);
    }
  }
}
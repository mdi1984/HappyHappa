using HappyHappa.REST.DAL;
using HappyHappa.REST.DAL.Model;
using System.Threading.Tasks;
using System.Web.Http;

namespace HappyHappa.REST.Controllers
{
  public class ItemController : AbstractController
  {
    public ItemController(IDAL dal) : base(dal) { }

    //// GET api/<controller>
    //public IEnumerable<string> Get()
    //{
    //  return new string[] { "value1", "value2" };
    //}

    //// GET api/<controller>/eggs
    //public string Get(string name)
    //{
    //  return "value";
    //}

    // PUT api/<controller>
    public async Task<Item> Put([FromBody] BoughtItem item)
    {
      return await Dal.PutItem(item);
    }

    // DELETE api/<controller>
    public async Task<Item> Delete([FromBody] BoughtItem item)
    {
      return await Dal.TakeItem(item);
    }
  }
}
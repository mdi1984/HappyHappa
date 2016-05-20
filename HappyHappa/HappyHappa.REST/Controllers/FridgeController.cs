using HappyHappa.REST.DAL;
using HappyHappa.REST.DAL.Model;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HappyHappa.REST.Controllers
{
  public class FridgeController : AbstractController
  {
    public FridgeController(IDAL dal) : base(dal) { }

    // POST api/<controller>
    public async Task<string> Post([FromBody] Device device)
    {
      try
      {
        return await Dal.CreateFridge(device);
      }
      catch (Exception e)
      {
        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
          Content = new StringContent(e.Message)
        });
      }
    }
  }
}
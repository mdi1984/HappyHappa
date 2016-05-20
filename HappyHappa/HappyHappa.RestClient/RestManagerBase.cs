using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HappyHappa.RestClient
{
  public class RestManagerBase
  {
    public async Task<object> PutWithJsonPayload(string url, object payload)
    {
      using (var client = new HttpClient())
      {
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var responseMessage = await client.PutAsync(
         url,
         new StringContent(
             jsonPayload.ToString(),
             Encoding.UTF8,
             "application/json"));

        return responseMessage;
      }
    }
  }
}

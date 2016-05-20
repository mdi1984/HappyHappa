using System.Net;
using System.Net.Http;

namespace HappyHappa.RestClient
{
  public class RestResponse
  {
    public RestResponse(HttpResponseMessage response)
    {
      this.StatusCode = response.StatusCode;
      this.Message = response.ToString();
    }

    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
  }
}
using System.Net;
using System.Net.Http;

namespace HappyHappa.RestClient
{
  public class RestResponse
  {
    public RestResponse(HttpResponseMessage response, string message)
    {
      this.StatusCode = response.StatusCode;
      this.Message = message;
      this.Response = response.ToString();
    }

    public RestResponse(HttpResponseMessage response)
    {
      this.StatusCode = response.StatusCode;
      this.Response = response.ToString();
    }

    public string Response { get; set; }

    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
  }
}
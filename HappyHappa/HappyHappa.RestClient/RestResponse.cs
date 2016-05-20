using System.Net.Http;

namespace HappyHappa.RestClient
{
  public class RestResponse
  {
    public RestResponse(HttpResponseMessage response)
    {
      this.StatusCode = int.Parse(response.StatusCode.ToString());
      this.Message = response.ToString();
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
  }
}
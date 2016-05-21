using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using GalaSoft.MvvmLight;
using HappyHappa.RestClient;

namespace HappyHappa.Pi.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {

    private string clientId;
    private ApplicationDataContainer localSettings;
    public LoginViewModel()
    {
      this.localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

      this.ClientId = localSettings.Values["ClientID"] as string;
      if (string.IsNullOrEmpty(this.ClientId))
      {
        this.LoginMessage = "Device not registered yet - establishing connection to registration service";
      }
      else
      {
        this.LoginMessage = "Loading...";
      }
    }

    private string loginMessage;

    public string LoginMessage
    {
      get { return this.loginMessage; }
      set { this.Set(ref loginMessage, value); }
    }


    public string ClientId
    {
      get { return this.clientId; }
      set
      {
        this.Set(ref clientId, value);
        App.ClientId = value;
      }
    }

    public async Task<RestResponse> TryRegisterDevice()
    {
      var restManager = new RestManagerBase();
      var deviceInformation = new EasClientDeviceInformation();
      var req = new 
      {
        DeviceAddress = deviceInformation.Id.ToString()
      };

      var registrationResult = await restManager.PostWithJsonPayload("http://happyhappa-uh.azurewebsites.net/api/fridge/", req);
      if (registrationResult.StatusCode != System.Net.HttpStatusCode.OK)
      {
        this.LoginMessage = "Unable to reach service";
      }
      return registrationResult;
    }

    public void SaveClientId(string clientId)
    {
      this.ClientId = clientId;
      this.localSettings.Values["ClientID"] = clientId;
    }
  }
}

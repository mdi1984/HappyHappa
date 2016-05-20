using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using GalaSoft.MvvmLight;
using HappyHappa.RestClient;

namespace HappyHappa.Pi.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {
    public LoginViewModel()
    {
      var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

      this.ClientId = localSettings.Values["ClientID"] as string;
    }

    private string clientId;

    public string ClientId
    {
      get { return this.clientId; }
      set { this.Set(ref clientId, value); }
    }

    public async Task<RestResponse> TryRegisterDevice()
    {
      var restManager = new RestManagerBase();
      var deviceInformation = new EasClientDeviceInformation();
      var req = new 
      {
        DeviceAddress = deviceInformation.Id.ToString()
      };

      var registrationResult = await restManager.PostWithJsonPayload("http://localhost:5039/api/fridge/", req);
      return registrationResult;
    }
  }
}

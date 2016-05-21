using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HappyHappa.Pi.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using HappyHappa.Pi.Messages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HappyHappa.Pi.Views
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class LoginPage : Page
  {
    public LoginPage()
    {
      this.InitializeComponent();
      this.Loaded += LoginPage_Loaded;
    }

    private async void LoginPage_Loaded(object sender, RoutedEventArgs e)
    {
      var vm = new LoginViewModel();
      this.DataContext = vm;

      if (!string.IsNullOrEmpty(vm.ClientId))
      {
        Messenger.Default.Send(new NavigateToMessage(typeof (InventoryPage)));
        // redirect to inventory page
      }
      else
      {
        // register new device and set clientId
        var result = await vm.TryRegisterDevice();
        vm.SaveClientId(result.Message.RemoveSpecialCharacters());
        Messenger.Default.Send(new NavigateToMessage(typeof(InventoryPage)));
      }
    }
  }
}

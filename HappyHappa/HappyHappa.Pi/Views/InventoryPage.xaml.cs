using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HappyHappa.Pi.AudioCapture;
using HappyHappa.Pi.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HappyHappa.Pi.Views
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class InventoryPage : Page
  {
    public HappaRecognizer HappaRecognizer { get; set; }

    public InventoryPage()
    {
      this.InitializeComponent();
      var viewModel = new MainViewModel();
      this.HappaRecognizer = new HappaRecognizer(viewModel, ResourceContext.GetForCurrentView());
      this.DataContext = viewModel;
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      await this.HappaRecognizer.Initialize();
      base.OnNavigatedTo(e);
    }
  }
}

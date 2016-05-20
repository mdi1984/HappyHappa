using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace HappyHappa.Pi.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    public MainViewModel()
    {
      this.TestStr = "Hello Happa";
    }

    private string testStr;

    public string TestStr
    {
      get { return this.testStr; }
      set { this.Set(ref testStr, value); }
    }

  }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using HappyHappa.Pi.Model;

namespace HappyHappa.Pi.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    public MainViewModel()
    {
      this.Items = new ObservableCollection<SimpleItem>();
    }

    public ObservableCollection<SimpleItem> Items { get; set; }

    private string cmdRecognizerState;

    public string CmdRecognizerState
    {
      get { return this.cmdRecognizerState; }
      set { this.Set(ref cmdRecognizerState, value); }
    }

    private string itemRecognizerState;

    public string ItemRecognizerState
    {
      get { return this.itemRecognizerState; }
      set { this.Set(ref itemRecognizerState, value); }
    }

    private string debugMessage;

    public string DebugMessage
    {
      get { return this.debugMessage; }
      set { this.Set(ref debugMessage, value); }
    }

    private string itemUnderstood;

    public string ItemUnderstood
    {
      get { return this.itemUnderstood; }
      set { this.Set(ref itemUnderstood, value); }
    }

    private string dateUnderstood;

    public string DateUnderstood
    {
      get { return this.dateUnderstood; }
      set { this.Set(ref dateUnderstood, value); }
    }

    public SimpleItem LastItem { get; set; }

    public void CommitLastItem()
    {
      this.Items.Add(this.LastItem);
      this.LastItem = null;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using GalaSoft.MvvmLight;
using HappyHappa.Pi.AudioCapture;
using HappyHappa.Pi.Model;
using HappyHappa.RestClient;

namespace HappyHappa.Pi.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private Dictionary<HappaState, ObservableCollection<String>> stateDictionary;
    public MainViewModel()
    {
      this.Items = new ObservableCollection<SimpleItem>();
      this.InitializeStateOptions();
    }

  public ObservableCollection<SimpleItem> Items { get; set; }

    private ObservableCollection<string> availableCommands;

    public ObservableCollection<string> AvailableCommands
    {
      get { return this.availableCommands; }
      set { this.Set(ref availableCommands, value); }
    }

    private string state;

    public string State
    {
      get { return this.state; }
      set { this.Set(ref state, value); }
    }

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

    private SimpleItem lastSavedItem;

    public SimpleItem LastSavedItem
    {
      get { return this.lastSavedItem; }
      set { this.Set(ref lastSavedItem, value); }
    }

    public SimpleItem LastItem { get; set; }
    public SimpleItem LastItemToRemove { get; set; }

    public async Task SaveLastItem()
    {
      this.LastItem.FridgeId = App.ClientId;
      await this.SendItemAsync(this.LastItem);
      this.Items.Add(this.LastItem);
      this.LastItem = null;
    }

    public async Task RemoveLastItem()
    {
      this.LastItemToRemove.FridgeId = App.ClientId;
      await this.SendItemDeletionAsync(this.LastItemToRemove);
      this.LastItemToRemove = null;
    }

    private async Task SendItemDeletionAsync(SimpleItem item)
    {
      var url = "http://localhost:5039/api/item/";
      var restManager = new RestManagerBase();
      var result = await restManager.DeleteWithJsonPayload(url, item);
    }

    private async Task SendItemAsync(SimpleItem item)
    {
      var url = "http://localhost:5039/api/item/";
      var restManager = new RestManagerBase();
      var result = await restManager.PutWithJsonPayload(url, item);
      if (result.StatusCode == System.Net.HttpStatusCode.OK)
      {
        this.LastSavedItem = item;
      }
    }

    private void InitializeStateOptions()
    {
      var commandMap = ResourceManager.Current.MainResourceMap.GetSubtree("Commands");
      var context = ResourceContext.GetForCurrentView();
      var mainCommandOptions = new string[]
      {
        commandMap.GetValue("NewItemsCommand", context).ValueAsString,
        commandMap.GetValue("RemoveItemsCommand", context).ValueAsString,
        commandMap.GetValue("CancelCommand", context).ValueAsString
      };

      var itemCreationOptions = new string[]
      {
        "{Anzahl} [{Einheit}] {Bezeichnung}",
        commandMap.GetValue("NewItemsOkayCommand", context).ValueAsString,
        commandMap.GetValue("CancelCommand", context).ValueAsString,
      };

      var itemDeletionOptions = new string[]
      {
        "{Anzahl} [{Einheit}] {Bezeichnung}",
        commandMap.GetValue("NewItemsOkayCommand", context).ValueAsString,
        commandMap.GetValue("CancelCommand", context).ValueAsString,
      };

      var expirationDateOptions = new string[]
      {
        "{Datum}",
        "Nein",
        commandMap.GetValue("CancelCommand", context).ValueAsString
      };

      this.stateDictionary = new Dictionary<HappaState, ObservableCollection<string>>();
      stateDictionary.Add(HappaState.Initializing, new ObservableCollection<string>());
      stateDictionary.Add(HappaState.WaitingForHeyHappa, new ObservableCollection<string>(new string[] { commandMap.GetValue("HeyHappaCommand", context).ValueAsString }));
      stateDictionary.Add(HappaState.WaitingForCommand, new ObservableCollection<string>(mainCommandOptions));
      stateDictionary.Add(HappaState.WaitingForItemCreation, new ObservableCollection<string>(itemCreationOptions));
      stateDictionary.Add(HappaState.WaitingForItemDeletion, new ObservableCollection<string>(itemDeletionOptions));
      stateDictionary.Add(HappaState.WaitingForExpirationDate, new ObservableCollection<string>(expirationDateOptions));
    }

    public void UpdateState(HappaState state)
    {
      this.State = state.ToString();
      this.AvailableCommands = this.stateDictionary[state];
    }
  }
}

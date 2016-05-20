using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using HappyHappa.Pi.Model;
using HappyHappa.Pi.ViewModels;

namespace HappyHappa.Pi.AudioCapture
{
  public class HappaRecognizer
  {
    private SpeechRecognizer cmdRecognizer;
    private SpeechRecognizer itemRecognizer;

    private CoreDispatcher dispatcher;
    private IAsyncOperation<SpeechRecognitionResult> recognitionOperation;
    private ResourceContext resContext;
    private ResourceMap commandMap;

    private MainViewModel vm;
    private Language lang;

    public HappaRecognizer(MainViewModel viewModel, ResourceContext ctx)
    {
      this.vm = viewModel;
      this.resContext = ctx;
      this.commandMap = ResourceManager.Current.MainResourceMap.GetSubtree("Commands");
    }

    //public HappaState AppState { get; set; }
    private HappaState appState;

    public HappaState AppState
    {
      get { return appState; }
      set { appState = value; }
    }



    public async Task Initialize()
    {
      this.dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
      this.SetDebugMessage("HappaRecognizer Initialize");
      this.AppState = HappaState.Initializing;
      var permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
      if (permissionGained)
      {
        this.lang = SpeechRecognizer.SupportedGrammarLanguages.FirstOrDefault(l => l.LanguageTag.Equals("de-DE"));
        var langTag = lang.LanguageTag;
        await this.InitializeCmdRecognizer();
        await this.InitializeItemRecognizer();

        await this.WaitForInitialCommand();
      }
    }

    #region Recognizer initialization

    private async Task InitializeCmdRecognizer()
    {
      CleanupCmdRecognizer();
      try
      {
        this.cmdRecognizer = new SpeechRecognizer(lang);
        this.cmdRecognizer.StateChanged += CmdRecognizerStateChanged;

        this.cmdRecognizer.Constraints.Add(
          new SpeechRecognitionListConstraint(new[]
          {
            this.GetResourceString("HeyHappaCommand"),
            this.GetResourceString("CancelCommand"),
            this.GetResourceString("NewItemsCommand"),
            this.GetResourceString("RemoveItemsCommand"),

          }));

        var result = await cmdRecognizer.CompileConstraintsAsync();
        if (result.Status != SpeechRecognitionResultStatus.Success)
        {
          this.SetDebugMessage("Grammar Compilation Failed: " + result.Status.ToString());
        }

        this.cmdRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed; ;
        this.cmdRecognizer.ContinuousRecognitionSession.ResultGenerated += OnCmdRecognizerResult;
      }
      catch (Exception ex)
      {
        // TODO: handle speech initialization errors
        this.SetDebugMessage(ex.Message);
      }
    }

    private void CleanupCmdRecognizer()
    {
      if (cmdRecognizer != null)
      {
        // cleanup prior to re-initializing cmdRecognizer
        this.cmdRecognizer.StateChanged -= CmdRecognizerStateChanged;

        this.cmdRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
        this.cmdRecognizer.ContinuousRecognitionSession.ResultGenerated -= OnCmdRecognizerResult;

        this.cmdRecognizer.Dispose();
        this.cmdRecognizer = null;
      }
    }

    private async Task InitializeItemRecognizer()
    {
      CleanupItemRecognizer();

      try
      {
        this.itemRecognizer = new SpeechRecognizer(lang);
        this.itemRecognizer.StateChanged += ItemRecognizerStateChanged;

        var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
        this.itemRecognizer.Constraints.Add(dictationConstraint);

        var result = await itemRecognizer.CompileConstraintsAsync();
        if (result.Status != SpeechRecognitionResultStatus.Success)
        {
          this.SetDebugMessage("Grammar Compilation Failed: " + result.Status.ToString());
        }

        this.itemRecognizer.StateChanged += ItemRecognizerStateChanged;
        this.itemRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed; ;
        this.itemRecognizer.ContinuousRecognitionSession.ResultGenerated += OnItemRecognizerResult;

        this.itemRecognizer.Timeouts.InitialSilenceTimeout = new TimeSpan(0);
        this.itemRecognizer.Timeouts.BabbleTimeout = new TimeSpan(0);
        this.itemRecognizer.Timeouts.EndSilenceTimeout = new TimeSpan(0);
        this.itemRecognizer.ContinuousRecognitionSession.AutoStopSilenceTimeout =
          new TimeSpan(0);
      }
      catch (Exception ex)
      {
        // TODO: handle speech initialization errors
        this.SetDebugMessage(ex.Message);
      }
    }

    private void CleanupItemRecognizer()
    {
      if (itemRecognizer != null)
      {
        // cleanup prior to re-initializing itemRecognizer
        this.itemRecognizer.StateChanged -= ItemRecognizerStateChanged;

        this.itemRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
        this.itemRecognizer.ContinuousRecognitionSession.ResultGenerated += OnItemRecognizerResult;

        this.itemRecognizer.Dispose();
        this.itemRecognizer = null;
      }
    }

    #endregion

    #region Recognizer result handlers

    private async void OnCmdRecognizerResult(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
    {
      this.SetDebugMessage($"ContinuousRecognitionSession_ResultGenerated: {args.Result.Text}");

      if (args.Result.Text.Equals(this.GetResourceString("CancelCommand")))
      {
        await this.SwitchToIdleMode();
        return;
      }

      if (this.AppState == HappaState.WaitingForHeyHappa)
      {
        if (args.Result.Text.Equals(this.GetResourceString("HeyHappaCommand")))
        {
          this.AppState = HappaState.WaitingForCommand;
        }
      }
      else if (this.AppState == HappaState.WaitingForCommand)
      {
        if (args.Result.Text.Equals(this.GetResourceString("NewItemsCommand")))
        {
          await this.SwitchToItemRecognition();
        }
        else if (args.Result.Text.Equals(this.GetResourceString("RemoveItemsCommand")))
        {
          await this.SwitchToItemRecognition(true);
        }
      }
    }

    private async void OnItemRecognizerResult(SpeechContinuousRecognitionSession sender,
      SpeechContinuousRecognitionResultGeneratedEventArgs args)
    {
      if (this.AppState == HappaState.WaitingForItemCreation)
      {
        if (args.Result.Text.Contains(this.GetResourceString("NewItemsOkayCommand")))
        {
          this.AppState = HappaState.WaitingForExpirationDate;
          this.SetDebugMessage("expiration date?");
        }
        else if (args.Result.Text.Contains(this.GetResourceString("CancelCommand")))
        {
          await this.SwitchToIdleMode();
        }
        else
        {
          SimpleItem item = null;
          var itemParseResult = ItemParser.TryParseItem(args.Result.Text, out item);
          await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
          {
            if (item != null)
            {
              this.vm.ItemUnderstood = args.Result.Text;
              this.vm.LastItem = item;
            }
            else
            {
              this.vm.ItemUnderstood = this.GetResourceString("NotUnderstood");
            }
          });
        }
      }
      else if (this.AppState == HappaState.WaitingForExpirationDate)
      {
        if (args.Result.Text.Contains(this.GetResourceString("NoCommand")) || args.Result.Text.Contains(this.GetResourceString("NewItemsOkayCommand")))
        {
          await this.vm.SaveLastItem();
          this.AppState = HappaState.WaitingForItemCreation;
        }
        else if (args.Result.Text.Contains(this.GetResourceString("CancelCommand")))
        {
          this.AppState = HappaState.WaitingForItemCreation;
          this.vm.LastItem = null;
        }
        else
        {
          DateTime expirationDate;
          // TODO if time left...: Format Provider
          var parseResult = DateTime.TryParse(args.Result.Text, out expirationDate);
          if (parseResult)
          {
            await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
              this.vm.DateUnderstood = expirationDate.ToString("dd.MM.yyyy");
              this.vm.LastItem.ExpirationDate = expirationDate;
            });
          }
          else
          {
            await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
              this.vm.DateUnderstood = this.GetResourceString("NotUnderstood");
            });
          }
        }
      }
      else if (this.AppState == HappaState.WaitingForItemDeletion)
      {
        if (args.Result.Text.Contains(this.GetResourceString("NewItemsOkayCommand")))
        {
          if (this.vm.LastItemToRemove != null)
          {
            await this.vm.RemoveLastItem();
          }
        }
        else
        {
          SimpleItem item = null;
          var itemParseResult = ItemParser.TryParseItem(args.Result.Text, out item);
          await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
          {
            if (item != null)
            {
              this.vm.ItemUnderstood = args.Result.Text;
              this.vm.LastItemToRemove = item;
            }
            else
            {
              this.vm.ItemUnderstood = this.GetResourceString("NotUnderstood");
            }
          });
        }
      }
    }

    private void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
    {
      this.SetDebugMessage($"ContinuousRecognitionSession_ResultGenerated: {args.Status.ToString()}");
    }

    #endregion


    private async Task SwitchToItemRecognition(bool delete = false)
    {
      try
      {
        await this.cmdRecognizer.ContinuousRecognitionSession.StopAsync();
        await itemRecognizer.ContinuousRecognitionSession.StartAsync();
        this.AppState = delete ? HappaState.WaitingForItemDeletion : HappaState.WaitingForItemCreation;
      }
      catch (Exception ex)
      {
        this.SetDebugMessage(ex.Message);
      }
    }

    // TODO: Obsolete ? 
    private async Task SwitchToCommandRecognition()
    {
      try
      {
        await this.itemRecognizer.ContinuousRecognitionSession.StopAsync();
        await this.cmdRecognizer.ContinuousRecognitionSession.StartAsync();
        this.AppState = HappaState.WaitingForCommand;
      }
      catch (Exception ex)
      {
        this.SetDebugMessage(ex.Message);
      }
    }

    private async Task SwitchToIdleMode()
    {
      try
      {
        this.AppState = HappaState.WaitingForHeyHappa;
        await this.itemRecognizer.ContinuousRecognitionSession.StopAsync();
        await this.cmdRecognizer.ContinuousRecognitionSession.StartAsync();
      }
      catch (Exception ex)
      {
        this.SetDebugMessage(ex.Message);
      }
    }

    private async Task WaitForInitialCommand()
    {
      this.AppState = HappaState.WaitingForHeyHappa;

      if (this.cmdRecognizer.State != SpeechRecognizerState.Idle)
        return;

      try
      {
        await cmdRecognizer.ContinuousRecognitionSession.StartAsync();

      }
      catch (Exception ex)
      {
        this.SetDebugMessage(ex.Message);
      }
    }

    private async void CmdRecognizerStateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
    {
      await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
      {
        this.vm.CmdRecognizerState = args.State.ToString();
      });

      if (args.State == SpeechRecognizerState.Idle &&
          (this.appState == HappaState.WaitingForCommand || this.appState == HappaState.WaitingForHeyHappa))
      {
        var prevState = this.AppState;
        this.AppState = HappaState.Initializing;
        await this.InitializeCmdRecognizer();
        await this.cmdRecognizer.ContinuousRecognitionSession.StartAsync();
        this.AppState = prevState;
      }
    }

    private async void ItemRecognizerStateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
    {
      await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
      {
        this.vm.ItemRecognizerState = args.State.ToString();
      });

      if (args.State == SpeechRecognizerState.Idle && (this.AppState == HappaState.WaitingForItemCreation || this.AppState == HappaState.WaitingForExpirationDate))
      {
        var prevState = this.AppState;
        this.AppState = HappaState.Initializing;
        await this.InitializeItemRecognizer();
        await this.itemRecognizer.ContinuousRecognitionSession.StartAsync();
        this.AppState = prevState;
      }
    }

    private async void SetDebugMessage(string msg)
    {
      await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
      {
        this.vm.DebugMessage = msg;
      });
    }

    private string GetResourceString(string key)
    {
      return this.commandMap.GetValue(key, this.resContext).ValueAsString;
    }
  }
}

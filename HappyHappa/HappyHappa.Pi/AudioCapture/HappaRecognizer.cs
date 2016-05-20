using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Media.SpeechRecognition;

namespace HappyHappa.Pi.AudioCapture
{
  public class HappaRecognizer
  {
    private SpeechRecognizer speechRecognizer;

    public HappaRecognizer()
    {
    }

    public async Task Initialize()
    {
      var permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
      if (permissionGained)
      {
        var lang = SpeechRecognizer.SupportedGrammarLanguages.FirstOrDefault(l => l.LanguageTag.Equals("de-DE"));
        var langTag = lang.LanguageTag;

        try
        {
          this.speechRecognizer = new SpeechRecognizer(lang);
          this.speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;
          this.speechRecognizer.Constraints.Add(new SpeechRecognitionListConstraint(new[] {"Hey Happa"}));
          // TODO: Add Grammar constraints
        }
        catch (Exception ex)
        {
          // TODO: handle speech initialization errors
        }
      }
    }

    private void SpeechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
    {
      throw new NotImplementedException();
    }
  }
}

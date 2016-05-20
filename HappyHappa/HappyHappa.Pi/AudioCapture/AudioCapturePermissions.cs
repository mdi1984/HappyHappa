using System;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace HappyHappa.Pi.AudioCapture
{
  public class AudioCapturePermissions
  {
    private static int NoCaptureDevicesHResult = -1072845856;

    public async static Task<bool> RequestMicrophonePermission()
    {
      try
      {
        MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
        settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
        settings.MediaCategory = MediaCategory.Speech;
        MediaCapture capture = new MediaCapture();

        await capture.InitializeAsync(settings);
      }
      catch (TypeLoadException)
      {
        var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components are unavailable.");
        await messageDialog.ShowAsync();
        return false;
      }
      catch (UnauthorizedAccessException)
      {
        return false;
      }
      catch (Exception exception)
      {
        if (exception.HResult == NoCaptureDevicesHResult)
        {
          var messageDialog = new Windows.UI.Popups.MessageDialog("No Audio Capture devices are present on this system.");
          await messageDialog.ShowAsync();
          return false;
        }
        else
        {
          throw;
        }
      }
      return true;
    }
  }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using FHICORC.Core.Data;
using FHICORC.Data;
using FHICORC.Services.Interfaces;
using Xamarin.Essentials;

namespace FHICORC.Services
{
    public class DeviceFeedbackService : IDeviceFeedbackService
    {
        private readonly IPreferencesService _preferencesService;
        
        public DeviceFeedbackService(IPreferencesService preferencesService)
        {
            _preferencesService = preferencesService;
        }
        
        public void Vibrate()
        {
            try
            {
                if (!_preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.SCANNER_VIBRATION_SETTING)) return;
                
                Vibration.Vibrate();
            }
            catch (FeatureNotSupportedException)
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void Vibrate(double durationMs)
        {
            try
            {
                if (!_preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.SCANNER_VIBRATION_SETTING)) return;
                
                Vibration.Vibrate(durationMs);
            }
            catch (FeatureNotSupportedException)
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void PlaySound(string fileNameWithExtension)
        {
            try
            {
                if (!_preferencesService.GetUserPreferenceAsBoolean(PreferencesKeys.SCANNER_SOUND_SETTING)) return;
                
                Stream audioStream = typeof(App).GetTypeInfo().Assembly
                    .GetManifestResourceStream($"FHICORC.Resources.Sounds.{fileNameWithExtension}");
                var player = CrossSimpleAudioPlayer.Current;

                player.Load(audioStream);
                player.Play();
            }
            catch (NullReferenceException nre)
            {
                Debug.WriteLine($"Could not find audio file: {fileNameWithExtension} at FHICORC.Resources.Sounds.{fileNameWithExtension}");
                Debug.WriteLine(nre.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void PerformHapticFeedback(HapticFeedbackType type)
        {
            try
            {
                HapticFeedback.Perform(type);
            }
            catch (FeatureNotSupportedException)
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
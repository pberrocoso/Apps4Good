using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Speech.Tts;

namespace Sidercar.Droid.Services
{
    public class TextToSpeechService : Java.Lang.Object, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public TextToSpeechService() { }

        public void Speak(string text)
        {
            var ctx = Application.Context; // useful for many Android SDK features
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(ctx, this);
            }
            else
            {
                if (Build.VERSION.SdkInt >= Build.VERSION_CODES.LollipopMr1)
                {
                    speaker.Speak(toSpeak, QueueMode.Flush, null, null);
                }
                else
                {
                    speaker.Speak(toSpeak, QueueMode.Flush, null);
                }
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                if (Build.VERSION.SdkInt >= Build.VERSION_CODES.LollipopMr1)
                {
                    speaker.Speak(toSpeak, QueueMode.Flush, null, null);
                }
                else
                {
                    speaker.Speak(toSpeak, QueueMode.Flush, null);
                }
                
            }
        }
        #endregion
    }
}
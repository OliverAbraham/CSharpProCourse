using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomenetBase;
using HomenetClient;

namespace SpeechRecognitionCore3
{
	class Program
	{
		private static SpeechConfig _Config;
        //private static DataObjectsConnector _connector;
        //private static List<Datenobjekt> _allDataObjects;

        public static async Task RecognizeSpeechAsync()
        {
            using (var recognizer = new SpeechRecognizer(_Config))
            {
				recognizer.Properties.SetProperty(PropertyId.SpeechServiceConnection_RecoLanguage, "de-DE");
                Console.WriteLine("Say something...");

                // Starts speech recognition, and returns after a single utterance is recognized. The end of a
                // single utterance is determined by listening for silence at the end or until a maximum of 15
                // seconds of audio is processed.  The task returns the recognition text as result. 
                // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
                // shot recognition like command or query. 
                // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
                var result = await recognizer.RecognizeOnceAsync();

                // Checks result.
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"{result.Text}");
                    await ProcessSpokenSentence(result);
                    //await SpeakAsync(result.Text);
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you update the subscription info?");
                    }
                }
            }
        }

        private static async Task ProcessSpokenSentence(SpeechRecognitionResult result)
        {
            //if (result.Text.StartsWith("Licht"))
            //{
            //    if (_allDataObjects == null)
            //    {
            //        _connector = new DataObjectsConnector("http://192.168.0.4:5001");
            //        _allDataObjects = _connector.GetAll();
            //    }
            //    var Do = new HomenetBase.Datenobjekt();
            //    Do.Name = "AZ_DECKENLAMPE";
            //    Do.DOID = _connector.Get_DOID_by_name(_allDataObjects, Do.Name);
            //    Do.Toggle();
            //    _connector.SetDataObjectValue(Do);

            //    //var Client = new HausnetClient();
            //    //Client.HomeautomationServerUrl = "http://192.168.0.4:89";
            //    //Client.HomeautomationPassword = "******YOURPASSWORD******";
            //    //Client.Change_data_object_value("AZ_DECKENLAMPE", result.Text.Contains("an") ? "1" : "0");

            //    await SpeakAsync("Okay");
            //    return;
            //}
        }

        public static async Task SpeakAsync(string text)
        {
            using (var synthesizer = new SpeechSynthesizer(_Config))
            {
				synthesizer.Properties.SetProperty(PropertyId.SpeechServiceConnection_SynthLanguage, "de-DE");
                using (var result = await synthesizer.SpeakTextAsync(text))
                {
                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        //Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                    }
                }
            }
        }

        static void Main()
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key // and service region (e.g., "westus").
            _Config = SpeechConfig.FromSubscription("0c2b33a160b543a5b4ad3f009a6307ab", "westeurope");

            while(true)
			{
				RecognizeSpeechAsync().Wait();
			}
            Console.WriteLine("Please press <Return> to continue.");
            Console.ReadLine();
        }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace StartingWithSpeechRecognition
{
    class Program
    {
        static SpeechRecognitionEngine _recognizer = null;
        static ManualResetEvent manualResetEvent = null;
        static void Main(string[] args)
        {
            manualResetEvent = new ManualResetEvent(false);
            Console.WriteLine("To recognize speech, and write 'test' to the console, press 0");
            Console.WriteLine("To recognize speech and make sure the computer speaks to you, press 1");
            Console.WriteLine("To emulate speech recognition, press 2");
            Console.WriteLine("To recognize speech using Choices and GrammarBuilder.Append, press 3");
            Console.WriteLine("To recognize speech using a DictationGrammar, press 4");
            Console.WriteLine("To get a prompt building example, press 5");
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            char keychar = pressedKey.KeyChar;
            Console.WriteLine("You pressed '{0}'", keychar);
            switch (keychar)
            {
                case '0':
                    RecognizeSpeechAndWriteToConsole();
                    break;
                case '1':
                    RecognizeSpeechAndMakeSureTheComputerSpeaksToYou();
                    break;
                case '2':
                    EmulateRecognize();
                    break;
                case '3':
                    SpeechRecognitionWithChoices();
                    break;
                case '4':
                    SpeechRecognitionWithDictationGrammar();
                    break;
                case '5':
                    PromptBuilding();
                    break;
                default:
                    Console.WriteLine("You didn't press 0, 1, 2, 3, 4, or 5!");
                    Console.WriteLine("Press any key to continue . . .");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                    break;
            }
            if (keychar != '5')
            {
                manualResetEvent.WaitOne();
            }
            if (_recognizer != null)
            {
                _recognizer.Dispose();
            }

            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
        }
        #region Recognize speech and write to console
        static void RecognizeSpeechAndWriteToConsole()
        {
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("test"))); // load a "test" grammar
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("exit"))); // load a "exit" grammar
            _recognizer.SpeechRecognized += _recognizeSpeechAndWriteToConsole_SpeechRecognized; // if speech is recognized, call the specified method
            _recognizer.SpeechRecognitionRejected += _recognizeSpeechAndWriteToConsole_SpeechRecognitionRejected; // if recognized speech is rejected, call the specified method
            _recognizer.SetInputToDefaultAudioDevice(); // set the input to the default audio device
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous

        }
        static void _recognizeSpeechAndWriteToConsole_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "test")
            {
                Console.WriteLine("test");
            }
            else if (e.Result.Text == "exit")
            {
                manualResetEvent.Set();
            }
        }
        static void _recognizeSpeechAndWriteToConsole_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Speech rejected. Did you mean:");
            foreach (RecognizedPhrase r in e.Result.Alternates)
            {
                Console.WriteLine("    " + r.Text);
            }
        }
        #endregion

        #region Recognize speech and make sure the computer speaks to you (text to speech)
        static void RecognizeSpeechAndMakeSureTheComputerSpeaksToYou()
        {
            Console.WriteLine("say 'hello you' ! ");
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("hello you"))); // load a "hello computer" grammar
            _recognizer.SpeechRecognized += _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognized; // if speech is recognized, call the specified method
            _recognizer.SpeechRecognitionRejected += _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognitionRejected;
            _recognizer.SetInputToDefaultAudioDevice(); // set the input to the default audio device
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous
        }
        static void _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "hello you")
            {
                SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                speechSynthesizer.Speak("hello Oliver");
                speechSynthesizer.Dispose();
            }
            else
            {
                SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
                speechSynthesizer.Speak(e.Result.Text);
                speechSynthesizer.Dispose();
            }
            manualResetEvent.Set();
        }
        static void _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (e.Result.Alternates.Count == 0)
            {
                Console.WriteLine("No candidate phrases found.");
                return;
            }
            Console.WriteLine("Speech rejected. Did you mean:");
            foreach (RecognizedPhrase r in e.Result.Alternates)
            {
                Console.WriteLine("    " + r.Text);
            }
        }
        #endregion

        #region Emulate speech recognition
        static void EmulateRecognize()
        {
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("emulate speech"))); // load "emulate speech" grammar
            _recognizer.SpeechRecognized += _emulateRecognize_SpeechRecognized;

            _recognizer.EmulateRecognize("emulate speech");

        }
        static void _emulateRecognize_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "emulate speech")
            {
                Console.WriteLine("Speech was emulated!");
            }
            manualResetEvent.Set();
        }
        #endregion

        #region Speech recognition with Choices and GrammarBuilder.Append
        static void SpeechRecognitionWithChoices()
        {
            _recognizer = new SpeechRecognitionEngine();
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append("ich"); // add "I"
            grammarBuilder.Append(new Choices("mag", "hasse")); // load "like" & "dislike"
            grammarBuilder.Append(new Choices("Hunde", "Katzen", "Vögel", "Schlangen", "Fische", "Tiger", "Löwen", "Dinosaurier", "Elefanten")); // add animals
            _recognizer.LoadGrammar(new Grammar(grammarBuilder)); // load grammar
            _recognizer.SpeechRecognized += speechRecognitionWithChoices_SpeechRecognized;
            _recognizer.SetInputToDefaultAudioDevice(); // set input to default audio device
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech
        }

        static void speechRecognitionWithChoices_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Do you really " + e.Result.Words[1].Text + " " + e.Result.Words[2].Text + "?");
            manualResetEvent.Set();
        }
        #endregion

        #region Speech recognition with DictationGrammar
        static void SpeechRecognitionWithDictationGrammar()
        {
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("Ende")));
            _recognizer.LoadGrammar(new DictationGrammar());
            _recognizer.SpeechRecognized += speechRecognitionWithDictationGrammar_SpeechRecognized;
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        static void speechRecognitionWithDictationGrammar_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "Ende")
            {
                manualResetEvent.Set();
                return;
            }
            Console.WriteLine("You said: " + e.Result.Text);
        }
        #endregion

        #region Prompt building
        static void PromptBuilding()
        {
            PromptBuilder builder = new PromptBuilder();

            builder.StartSentence();
            builder.AppendText("Dies ist ein Beispiel.");
            builder.EndSentence();

            builder.StartSentence();
            builder.AppendText("Jetzt kommt eine Pause von 2 Sekunden.");
            builder.EndSentence();

            builder.AppendBreak(new TimeSpan(0, 0, 2));

            builder.StartStyle(new PromptStyle(PromptVolume.ExtraSoft));
            builder.AppendText("Extra leise.");
            builder.EndStyle();

            builder.StartStyle(new PromptStyle(PromptRate.Fast));
            builder.AppendText("Dies ist schnell gesprochen.");
            builder.EndStyle();

            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.Speak(builder);
            synthesizer.Dispose();
        }
        #endregion

    }
}

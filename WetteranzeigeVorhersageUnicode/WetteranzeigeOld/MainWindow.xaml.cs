﻿//-----------------------------------------------------------------------------
//
//                      Demo-Programm Wetteranzeige mit HTML Parser
//                      Oliver Abraham
//                      mail@oliver-abraham.de
//                      www.oliver-abraham.de
//
// Literatur:
// http://html-agility-pack.net
// https://stackoverflow.com/questions/1604471/how-can-i-find-an-element-by-css-class-with-xpath
//
//-----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using InternetHelper;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;

namespace Wetteranzeige
{
    public partial class MainWindow : Window
    {
        DispatcherTimer MeinTimer;
        string Homepage              = "http://www.wetter.de/deutschland/wetter-lentfoehrden-18219233.html";
        const string WetterboxAnfang = "base-box box-slider__box base-box--level-1"; //"box-slider__container";
        const string Ort             = "Lentföhrden";
        const string OrtAnfang       = Ort + "</a></td>";
        const string OrtEnde         = "</tr>";
        const string TempAnfang      = "<td width=\"40\">";
        const string TempEnde        = "</td>";
        const string Ortsangabe      = "Das Wetter in {0}:";

        public MainWindow()
        {
            InitializeComponent();
            Starte_Timer();
        }

        void Starte_Timer()
        {
            MeinTimer = new DispatcherTimer(new TimeSpan(1 * 1000 * 10000),    // in einer Sekunde
                                            DispatcherPriority.Normal,
                                            new EventHandler(MeinTimer_Elapsed),
                                            this.Dispatcher);
            MeinTimer.Start();
        }

        void MeinTimer_Elapsed(object sender, EventArgs e)
        {
            Update_Wetter();
            MeinTimer.Interval = new TimeSpan(30 * 1000 * 10000);  // wieder in 30 Sekunden
        }

        void Update_Wetter()
        {
            Downloader Loader = new Downloader();
            string pageContent = Loader.DownloadFromUrl(Homepage);
            
            var converter = new WeatherConverter();
            var weatherData = converter.ExtractWeatherDataFromPage(pageContent);

            double Temperature = converter.FindTemperatureForTime(weatherData, DateTime.Now);
            
            Ortsname  .Content = String.Format(Ortsangabe, Ort);
            Temperatur.Content = $"{Temperature} °";

            col1.Content = ConvertIconToUnicode(FindIconForTime(weatherData, new DateTime(2000,1,1, 7,0,0)));
            col2.Content = ConvertIconToUnicode(FindIconForTime(weatherData, new DateTime(2000,1,1,13,0,0)));
            col3.Content = ConvertIconToUnicode(FindIconForTime(weatherData, new DateTime(2000,1,1,19,0,0)));
            col4.Content = ConvertIconToUnicode(FindIconForTime(weatherData, new DateTime(2000,1,1,23,0,0)));

            temp1.Content = FindTemperatureForTime(weatherData, new DateTime(2000,1,1, 7,0,0)) + " °";
            temp2.Content = FindTemperatureForTime(weatherData, new DateTime(2000,1,1,13,0,0)) + " °";
            temp3.Content = FindTemperatureForTime(weatherData, new DateTime(2000,1,1,19,0,0)) + " °";
            temp4.Content = FindTemperatureForTime(weatherData, new DateTime(2000,1,1,23,0,0)) + " °";
        }
    }
}
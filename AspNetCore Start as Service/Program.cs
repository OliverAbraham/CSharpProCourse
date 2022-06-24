using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;



// ----------------------------------------------------------------------------------------------------
//
//				Asp.NET Anwendung als Windows Service starten
//
//
// 1. NuGet package "Microsoft.Extensions.Hosting.WindowsServices" hinzufügen
// 2. Eine Zeile hinzufügen: .UseWindowsService();
// 3. Anwendung kann ganz normal im Visual Studio debuggt werden
// 4. Anwendung kann ganz normal als Command line programm gestartet werden
// 5. Mit sc.exe wird es als Dienst installiert
//
//
//    QUELLE:
//
// https://www.ben-morris.com/running-a-net-core-console-application-as-a-windows-service/
//
//
//
//    VARIANTEN
//
// Es gibt 4 Varianten: (dieses Programm ist die letzte Variante)
// 	• Using a service manager like NSSM to manage the registration and running of the service.
// 	• Building a native service using the Windows Compatibility Pack for .Net Core
// 	• Creating a service using Topshelf
// 	• Using .Net Core’s generic host builder to run background services as a Windows Service
// Aus <https://www.ben-morris.com/running-a-net-core-console-application-as-a-windows-service/> 
// 
// 
// 
//    INSTALLATION
// 
// Service installieren
// sc create AspNetCoreTest3 binPath= "C:\Cloud\AspNetCoreServiceTest\AspNetCoreAlsServiceStarten.exe"
// 
// sc description AspnetCoreTest4 "Meine Beschreibung"
// sc config AspNetCoreTest4 DisplayName= "AspNetCoreTest4"
// 
// Service Starten
// sc start AspNetCoreTest3
// 
// Service Status Abfragen
// sc query AspNetCoreTest3
// 
// Service stoppen
// sc stop AspNetCoreTest3
// 
// Service Startmodus ändern
// sc config AspNetCoreTest4 start= auto
// sc config AspNetCoreTest4 start= disabled
// 
// Service entfernen
// sc delete AspNetCoreTest4
// 
// 
// 
//    TYPISCHE FEHLERMELDUNGEN
// 
// "Kann die angegebene Datei nicht finden"
// Keine Zugriffsrechte auf die Datei. 
// Die Installation im System32-Unterordner und im Programme-Verzeichnis ging ohne Probleme.
// Auch im C:\Cloud-Verzeichnis ging es.
// 
// "Der angegebene Dienst wurde zum Löschen markiert."
// Es hilft ein Windows-Neustart. Danach ist der Dienst weg.
// 
// 
// ----------------------------------------------------------------------------------------------------



namespace AspNetCoreAlsServiceStarten
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				
				// nur diese Zeile hinzufügen:
				.UseWindowsService();
	}
}

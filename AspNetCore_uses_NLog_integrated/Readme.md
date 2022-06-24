# RUN AN ASP.NET CORE APPLICATION WITH INTEGRATED NLOG INSTEAD OF MICROSOFT LOGGER

## Steps
   1. Add 2 Nuget packages: NLog, NLog.Web.AspNetCore
   2. Add Nuget package Nlog.Config or create your own NLog.config
   3. Add a using
   4. Add 1 line for nlog initialization
   5. Add "ConfigureLogging" to remove the internal log provider
   6. Add "UseNLog" to add our new provider
   7. Start application standalone (with kestrel), not in IIS

## Links
https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-5


## Author
Oliver Abraham, mail@oliver-abraham.de, www.oliver-abraham.de

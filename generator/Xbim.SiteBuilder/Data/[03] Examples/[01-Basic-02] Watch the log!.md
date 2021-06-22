﻿# Watch the log!

Xbim is logging processing errors and warnings where exceptions are not appropriate.
It is heavily used in Xbim Geometry Engine to log all geometry errors. It is also used in the parser to log any syntactic problems with the file. 
If you have a file which doesn't look quite right you should always check the log first.

As a library we don't want to force you to use any specific logging library. To stay agnostic of the logging framework we use 
[Microsoft Logging Abstractions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.abstractions?view=dotnet-plat-ext-3.1&viewFallbackFrom=netstandard-2.1).
Most of the common logging frameworks have their implementation of logging providers for these abstractions, which makes it easy to 
wire xbim in your wider application logging.

Setup is pretty easy. Just initialize `XbimLogging.LoggerFactory` before you do anything with xbim. We often use [Serilog](https://serilog.net/) in our examples,
which looks like this:

```xml
    <PackageReference Include="Serilog.Extensions.Logging" Version="2.0.2" />
    <PackageReference Include="Serilog.Sinks.Console" Version="3.0.1" />
```

```cs
// set up Serilog
Log.Logger = new LoggerConfiguration()
   .Enrich.FromLogContext()
   .WriteTo.Console()
   .CreateLogger();

// set up xbim logging. It will use your providers.
XbimLogging.LoggerFactory.AddSerilog();

// do anything you want with xbim
using (var model = IfcStore.Open(fileName, null, -1))
{
    // ...
}
```

## Xbim v4 and earlier

Xbim used to use [Log4Net](https://logging.apache.org/log4net/). If you want to use it with newer versions of xbim you can use 
[Log4Net provider](https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore) for Microsoft abstractions.

There are many possible configurations of Log4Net including logging to console, creating continuous or rolling log file and others.
Some examples can be found [here](https://logging.apache.org/log4net/release/config-examples.html). The most basic configuration which will write all colourful log 
messages in console can look like this:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>
  <log4net>
    <!-- Defines default logging behaviour -->
    <root>
      <appender-ref ref="console" />
      <!-- Set the default logging level to one of ALL DEBUG INFO WARN ERROR FATAL NONE -->
      <level value="ALL" />
    </root>
    <appender name="console" type="log4net.Appender.ColoredConsoleAppender">
      <mapping>
        <level value="FATAL" />
        <foreColor value="White" />
        <backColor value="Red" />
      </mapping>
      <mapping>
        <level value="ERROR" />
        <foreColor value="Red, HighIntensity" />
      </mapping>
      <mapping>
        <level value="WARN" />
        <foreColor value="Green, HighIntensity" />
      </mapping>
      <mapping>
        <level value="INFO" />
        <foreColor value="Blue, HighIntensity" />
      </mapping>
      <mapping>
        <level value="DEBUG" />
        <foreColor value="White" />
      </mapping>
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%-5level - %message  [%logger %type.%method Line %line]%newline" />
      </layout>
    </appender>
  </log4net>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
  </startup>
</configuration>
```

You can also use xbim infrastructure to log your own messages like this:

```cs
using Xbim.Common.Logging;
```

```cs
var log = LoggerFactory.GetLogger(); 

log.Info("Examples are just about to start.");
log.Warn("Always use LINQ instead of general iterations!");
log.Error("This is how the error would be logged with log4net.");
log.Info("All examples finished.");
```
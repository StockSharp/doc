# Strategy logging

The [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class implements the [ILogSource](xref:Ecng.Logging.ILogSource) interface. Therefore, the strategies can be passed to the [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources), and all of its messages will automatically get to the [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners). 

## Prerequisites

[Trading strategies](../strategies.md)

## Logging to a test file

1. First, you need to create the special manager: 

   ```cs
   var logManager = new LogManager();
   ```
2. Then you need to create a file logger, passing to it the name of the file and to add it to the [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners): 

   ```cs
   var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
   logManager.Listeners.Add(fileListener);
   ```
3. For logging messages, you need to add a strategy to the [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources): 

   ```cs
   logManager.Sources.Add(lkohSmaStrategy);
   ```
4. After adding strategy to the logging manager all of its messages will be recorded to file. 

## Sound playback

1. Creating a logger and passing the name of the sound file to it:

   ```cs
   var soundListener = new SoundLogListener("error.mp3");
   						
   logManager.Listeners.Add(soundListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Setting the filter so sound plays only when the message type is [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error):

   ```cs
   soundListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   ```

## Email sending

1. Create the logger and pass it the parameters for the sent letters:

   ```cs
   var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
   logManager.Listeners.Add(emailListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Setting the filter on the sending of messages of [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error) and [LogLevels.Warning](xref:Ecng.Logging.LogLevels.Warning) types: 

   ```cs
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Warning);
   ```

## Logging in to the LogWindow

1. Creating the [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) logger:

   ```cs
    // each strategy will have their own windows
   var guiListener = new GuiLogListener();
   logManager.Listeners.Add(guiListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Here is the log window when the strategy is working: ![strategylogging](../../../images/strategy_logging.png)

## Recommended content

[Visual logging components](../graphical_user_interface/logging.md)

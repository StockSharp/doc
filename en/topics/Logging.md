# Logging

For the monitoring of trading algorithms written in [S\#](StockSharpAbout.md), you can use the special [LogManager](../api/StockSharp.Logging.LogManager.html) class. This class receives the [LogMessage](../api/StockSharp.Logging.LogMessage.html) messages from the [LogManager.Sources](../api/StockSharp.Logging.LogManager.Sources.html) through the [ILogSource.Log](../api/StockSharp.Logging.ILogSource.Log.html) event and passes them to the [LogManager.Listeners](../api/StockSharp.Logging.LogManager.Listeners.html) listeners. Therefore, the algorithm code will be able to pass debug information (for example, about errors occurred during the operation, or additional information about mathematical calculations) and [LogManager](../api/StockSharp.Logging.LogManager.html) will decide how to display this information for the operator. 

Normally the [S\#](StockSharpAbout.md) contains the following implementations of the [ILogListener](../api/StockSharp.Logging.ILogListener.html), whose choice affects where messages received from strategies will be passed: 

1. [FileLogListener](../api/StockSharp.Logging.FileLogListener.html)

    \- writes messages into a text file. It is recommended to use for already created algorithm, and to use the logs in cases of force majeure. 
2. [SoundLogListener](../api/StockSharp.Xaml.SoundLogListener.html)

    \- plays the audio message when a new message arrives. It is recommended to use if the algorithm is not under continuous monitoring. 
3. [ConsoleLogListener](../api/StockSharp.Logging.ConsoleLogListener.html)

    \- outputs messages to the console window (if the algorithm does not have a window, it will be automatically created). It is recommended to use for debugging and testing the algorithm 
4. [DebugLogListener](../api/StockSharp.Logging.DebugLogListener.html)

    \- outputs messages to the debug window. This window can be viewed through special programs such as 

   [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx)

   . It is recommended to use for debugging and testing the algorithm. 
5. [EmailLogListener](../api/StockSharp.Logging.EmailLogListener.html)

    \- sends messages to the specified email address. It is recommended to use if the algorithm is located at not controlled computer (at hoster’s server). 
6. [GuiLogListener](../api/StockSharp.Xaml.GuiLogListener.html)

    \- displays messages through the special 

   [LogWindow](../api/StockSharp.Xaml.LogWindow.html)

    window. Able to work in two modes: when all messages output in a single window, and when a separate window is created for each 

   [ILogSource](../api/StockSharp.Logging.ILogSource.html)

   . It is recommended to use if the algorithm has a graphical interface. 

[LogListener](../api/StockSharp.Logging.LogListener.html) can be configured to filter messages through the [Filters](../api/StockSharp.Logging.LogListener.Filters.html) property. For example, through the filters you can specify what type of messages should be processed. This is particularly useful when the [SoundLogListener](../api/StockSharp.Xaml.SoundLogListener.html) or the [EmailLogListener](../api/StockSharp.Logging.EmailLogListener.html) is used, in order that, for example, to send e\-mail in emergency situations only (trade algorithm error) rather than on each debug message. 

### Next Steps

[Strategy logging](LoggingStrategy.md)

[IConnector logging](LoggingITrader.md)

[Other logs sources](AppLogging.md)

[Visual monitoring](LoggingMonitorWindow.md)

[ILogListener creating](LoggingCustomListener.md)

## Recommended content

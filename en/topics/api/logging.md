# Logging

For the monitoring of trading algorithms written in [S\#](../api.md), you can use the special [LogManager](xref:Ecng.Logging.LogManager) class. This class receives the [LogMessage](xref:Ecng.Logging.LogMessage) messages from the [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) through the [ILogSource.Log](xref:Ecng.Logging.ILogSource.Log) event and passes them to the [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) listeners. Therefore, the algorithm code will be able to pass debug information (for example, about errors occurred during the operation, or additional information about mathematical calculations) and [LogManager](xref:Ecng.Logging.LogManager) will decide how to display this information for the operator. 

Normally the [S\#](../api.md) contains the following implementations of the [ILogListener](xref:Ecng.Logging.ILogListener), whose choice affects where messages received from strategies will be passed: 

1. [FileLogListener](xref:Ecng.Logging.FileLogListener) \- writes messages into a text file. It is recommended to use for already created algorithm, and to use the logs in cases of force majeure. 
2. [ConsoleLogListener](xref:Ecng.Logging.ConsoleLogListener) \- outputs messages to the console window (if the algorithm does not have a window, it will be automatically created). It is recommended to use for debugging and testing the algorithm 
3. [DebugLogListener](xref:Ecng.Logging.DebugLogListener) \- outputs messages to the debug window. This window can be viewed through special programs such as [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx). It is recommended to use for debugging and testing the algorithm. 
4. [EmailLogListener](xref:Ecng.Logging.EmailLogListener) \- sends messages to the specified email address. It is recommended to use if the algorithm is located at not controlled computer (at hoster’s server). 
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) \- displays messages through the special [LogControl](xref:StockSharp.Xaml.LogControl) window. Able to work in two modes: when all messages output in a single window, and when a separate window is created for each [ILogSource](xref:Ecng.Logging.ILogSource). It is recommended to use if the algorithm has a graphical interface. 

[LogListener](xref:Ecng.Logging.LogListener) can be configured to filter messages through the [LogListener.Filters](xref:Ecng.Logging.LogListener.Filters) property. For example, through the filters you can specify what type of messages should be processed. This is particularly useful when the [EmailLogListener](xref:Ecng.Logging.EmailLogListener) is used, in order that, for example, to send e\-mail in emergency situations only (trade algorithm error) rather than on each debug message. 

## Next Steps

[Strategy logging](logging/strategy_logging.md)

[IConnector logging](logging/iconnector_logging.md)

[Other logs sources](logging/other_logs_sources.md)

[Visual monitoring](logging/visual_monitoring.md)

[ILogListener creating](logging/custom_iloglistener.md)

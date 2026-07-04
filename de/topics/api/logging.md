# Logging

Für das Monitoring von in [S#](../api.md) geschriebenen Handelsalgorithmen können Sie die spezielle Klasse [LogManager](xref:Ecng.Logging.LogManager) verwenden. Diese Klasse empfängt Nachrichten [LogMessage](xref:Ecng.Logging.LogMessage) aus [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) über das Ereignis [ILogSource.Log](xref:Ecng.Logging.ILogSource.Log) und übergibt sie an die Listener [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners). Dadurch kann der Algorithmuscode Debug-Informationen übergeben (zum Beispiel über Fehler während der Ausführung oder zusätzliche Informationen zu mathematischen Berechnungen), und [LogManager](xref:Ecng.Logging.LogManager) entscheidet, wie diese Informationen dem Operator angezeigt werden.

Normalerweise enthält [S#](../api.md) die folgenden Implementierungen von [ILogListener](xref:Ecng.Logging.ILogListener). Die Auswahl bestimmt, wohin von Strategien empfangene Nachrichten weitergeleitet werden:

1. [FileLogListener](xref:Ecng.Logging.FileLogListener) - schreibt Nachrichten in eine Textdatei. Empfohlen für bereits erstellte Algorithmen und für Logs in Fällen höherer Gewalt.
2. [ConsoleLogListener](xref:Ecng.Logging.ConsoleLogListener) - gibt Nachrichten im Konsolenfenster aus (wenn der Algorithmus kein Fenster hat, wird es automatisch erstellt). Empfohlen zum Debuggen und Testen des Algorithmus.
3. [DebugLogListener](xref:Ecng.Logging.DebugLogListener) - gibt Nachrichten im Debugfenster aus. Dieses Fenster kann über spezielle Programme wie [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx) angezeigt werden. Empfohlen zum Debuggen und Testen des Algorithmus.
4. [EmailLogListener](xref:Ecng.Logging.EmailLogListener) - sendet Nachrichten an die angegebene E-Mail-Adresse. Empfohlen, wenn der Algorithmus auf einem nicht direkt kontrollierten Computer läuft (auf einem Server des Hosters).
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) - zeigt Nachrichten über das spezielle Fenster [LogControl](xref:StockSharp.Xaml.LogControl) an. Kann in zwei Modi arbeiten: Ausgabe aller Nachrichten in einem einzelnen Fenster oder Erstellung eines separaten Fensters für jede [ILogSource](xref:Ecng.Logging.ILogSource). Empfohlen, wenn der Algorithmus eine grafische Oberfläche hat.

[LogListener](xref:Ecng.Logging.LogListener) kann über die Eigenschaft [LogListener.Filters](xref:Ecng.Logging.LogListener.Filters) zum Filtern von Nachrichten konfiguriert werden. Über Filter können Sie beispielsweise festlegen, welche Nachrichtentypen verarbeitet werden sollen. Dies ist besonders nützlich bei Verwendung von [EmailLogListener](xref:Ecng.Logging.EmailLogListener), um E-Mails nur in Notfällen (Fehler im Handelsalgorithmus) und nicht bei jeder Debug-Nachricht zu senden.

## Nächste Schritte

[Strategy logging](logging/strategy_logging.md)

[IConnector logging](logging/iconnector_logging.md)

[Other logs sources](logging/other_logs_sources.md)

[Visual monitoring](logging/visual_monitoring.md)

[ILogListener creating](logging/custom_iloglistener.md)


# Strategie-Logging

Die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) implementiert das Interface [ILogSource](xref:Ecng.Logging.ILogSource). Daher können Strategien an [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) übergeben werden, und alle ihre Nachrichten gelangen automatisch zu [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners).

## Voraussetzungen

[Handelsstrategien](../strategies.md)

## Logging in eine Testdatei

1. Zuerst müssen Sie den speziellen Manager erstellen:

   ```cs
   var logManager = new LogManager();
   ```
2. Danach müssen Sie einen Datei-Logger erstellen, ihm den Dateinamen übergeben und ihn zu [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) hinzufügen:

   ```cs
   var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
   logManager.Listeners.Add(fileListener);
   ```
3. Zum Loggen von Nachrichten müssen Sie die Strategie zu [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) hinzufügen:

   ```cs
   logManager.Sources.Add(lkohSmaStrategy);
   ```
4. Nach dem Hinzufügen der Strategie zum Logging-Manager werden alle ihre Nachrichten in die Datei geschrieben.

## Soundwiedergabe

1. Erstellen eines Loggers und Übergabe des Namens der Sounddatei:

   ```cs
   var soundListener = new SoundLogListener("error.mp3");

   logManager.Listeners.Add(soundListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Filter so setzen, dass der Sound nur abgespielt wird, wenn der Nachrichtentyp [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error) ist:

   ```cs
   soundListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   ```

## E-Mail-Versand

1. Erstellen Sie den Logger und übergeben Sie ihm die Parameter für die versendeten Nachrichten:

   ```cs
   var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
   logManager.Listeners.Add(emailListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. Setzen des Filters für das Senden von Nachrichten der Typen [LogLevels.Error](xref:Ecng.Logging.LogLevels.Error) und [LogLevels.Warning](xref:Ecng.Logging.LogLevels.Warning):

   ```cs
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
   emailListener.Filters.Add(msg => msg.Level == LogLevels.Warning);
   ```

## Logging in das LogWindow

1. Erstellen des [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener)-Loggers:

   ```cs
    // Jede Strategie erhält ihr eigenes Fenster
   var guiListener = new GuiLogListener();
   logManager.Listeners.Add(guiListener);
   logManager.Sources.Add(lkohSmaStrategy);
   ```
2. So sieht das Logfenster während der Arbeit der Strategie aus: ![Strategie-Logging Screenshot](../../../images/strategy_logging.png)

## Empfohlene Inhalte

[Visuelle Logging-Komponenten](../graphical_user_interface/logging.md)

